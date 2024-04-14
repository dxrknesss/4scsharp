using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace practice6
{
    internal class NetworkManager
    {
        public static UdpClient Client;
        public static PeerType CurrentPeerType;
        public static IPEndPoint RemoteConnectionPoint;

        public enum PacketType
        {
            PT_HELLO, PT_ACK, PT_POINT, PT_READY, PT_CHNST
        }

        public enum PeerType 
        {
            HOST, CLIENT
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

            Client = new UdpClient() { EnableBroadcast = true };
            RemoteConnectionPoint = remotePoint;
        }

        public static void SetClient(int port)
        {
            Client = new UdpClient(port) { EnableBroadcast = true };
        }

        async public static Task<bool> ReceivePacketAsync(PacketType desiredType)
        {
            var result = await Client.ReceiveAsync();
            var endpoint = result.RemoteEndPoint;
            var bufferDeserialized = JsonSerializer.Deserialize<PacketType>(result.Buffer);

            if (bufferDeserialized == desiredType)
            {
                await SendPacketTypeAsync(PacketType.PT_ACK, null, endpoint); // TODO: refactor all the shit outta here
                return true;
            }
            return false;
        }

        public static bool ReceivePacketSync(PacketType desiredType)
        {
            var result = Client.Receive(ref RemoteConnectionPoint);
            var bufferDeserialized = JsonSerializer.Deserialize<PacketType>(result);

            if (bufferDeserialized == desiredType)
            {
                SendPacketTypeSync(PacketType.PT_ACK, null, RemoteConnectionPoint); // TODO: refactor all the shit outta here
                return true;
            }
            return false;
        }

        async internal static Task<bool> SendPacketTypeAsync(PacketType request, PacketType? desiredResponse,
            IPEndPoint endpoint)
        {
            byte[] payload = JsonSerializer.SerializeToUtf8Bytes(request);
            int timeout = 1500;

            await Client.SendAsync(payload, payload.Length, endpoint);

            using var cst = new CancellationTokenSource(timeout);
            try
            {
                var result = await Client.ReceiveAsync();
                PacketType pt = JsonSerializer.Deserialize<PacketType>(result.Buffer);
                if (desiredResponse != null && pt != desiredResponse)
                {
                    throw new InvalidOperationException();
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static bool SendPacketTypeSync(PacketType request, PacketType? desiredResponse,
            IPEndPoint endpoint)
        {
            byte[] payload = JsonSerializer.SerializeToUtf8Bytes<PacketType>(request);
            int timeout = 1500;

            Client.Send(payload, payload.Length, endpoint);

            try
            {
                Task<byte[]> receiveRes = Task.Run(() =>
                {
                    var result = Client.Receive(ref endpoint);
                    return result;
                });

                Task.WaitAny(new Task[] { receiveRes, Task.Delay(1500) });

                if (!receiveRes.IsCompleted)
                {
                    throw new OperationCanceledException();
                }
                PacketType pt = JsonSerializer.Deserialize<PacketType>(receiveRes.Result);

                if (pt != desiredResponse)
                {
                    throw new InvalidOperationException();
                }
                return true;
            }
            catch (Exception)
            {
                // SendPacketTypeSync(request, desiredResponse, endpoint);
                // todo: display error message!
                return false;
            }
        }
    }
}
