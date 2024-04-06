using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace practice6
{
    internal class NetworkManager
    {
        public static UdpClient Client;
        public static string LobbyName;

        public enum PacketType
        {
            PT_HELLO, PT_ACK, PT_CHANGESTATE, PT_POINT
        }

        public static void SetClient(string ipAddr, int port)
        {
            IPEndPoint remotePoint;
            try
            {
                remotePoint = new IPEndPoint(IPAddress.Parse(ipAddr), port);
            }
            catch (FormatException exception)
            {
                throw exception;
            }

            Client = new UdpClient(remotePoint);
            Client.EnableBroadcast = true;
        }

        public static void SetClient(int port)
        {
            Client = new UdpClient(port);
            Client.EnableBroadcast = true;
        }

        public static void SendHello()
        {
            SendPacketType(PacketType.PT_HELLO, PacketType.PT_ACK);
        }

        async public static void ReceiveHello()
        {
            var result = await Client.ReceiveAsync();
            var endpoint = result.RemoteEndPoint;
            var bufferDeserialized = JsonSerializer.Deserialize<PacketType>(result.Buffer);

            
            if (bufferDeserialized == PacketType.PT_HELLO)
            {
                SendPacketType(PacketType.PT_ACK, PacketType.PT_CHANGESTATE, endpoint);
            }
        }

        async private static void SendPacketType(PacketType request, PacketType desiredResponse,
            IPEndPoint endpoint = null)
        {
            byte[] payload = JsonSerializer.SerializeToUtf8Bytes<PacketType>(request);
            int timeout = 2000;
            using (var cst = new CancellationTokenSource(timeout))
            {
                await Client.SendAsync(payload, payload.Length, endpoint ?? (IPEndPoint)Client.Client.RemoteEndPoint);

                try
                {
                    var result = await Client.ReceiveAsync();
                    PacketType pt = JsonSerializer.Deserialize<PacketType>(result.Buffer);
                    if (pt != desiredResponse)
                    {
                        throw new InvalidOperationException();
                    }
                }
                catch (OperationCanceledException)
                {
                    SendPacketType(request, desiredResponse);
                }
                catch (InvalidOperationException)
                {
                    return;
                }
            }
        }
    }
}
