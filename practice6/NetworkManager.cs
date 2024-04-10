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
        public static string LobbyName;
        public static IPEndPoint RemoteConnectionPoint;
        public static string BroadcastAddress = "192.168.194.255";

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

            Client = new UdpClient();
            RemoteConnectionPoint = remotePoint;
            Client.EnableBroadcast = true;
        }

        public static void SetClient(int port)
        {
            Client = new UdpClient(port);
            Client.EnableBroadcast = true;
        }

        public static void SendHello(bool sync) // todo: refactor to the form of loosely coupled method
        {
            if (sync) // todo: rewrite with delegates
            {
                SendPacketTypeSync(PacketType.PT_HELLO, PacketType.PT_ACK, RemoteConnectionPoint);
            } 
            else
            {
                SendPacketType(PacketType.PT_HELLO, PacketType.PT_ACK, RemoteConnectionPoint);
            }    
        }

        async public static Task<bool> ReceivePacketAsync(PacketType desiredType)
        {
            var result = await Client.ReceiveAsync();
            var endpoint = result.RemoteEndPoint;
            var bufferDeserialized = JsonSerializer.Deserialize<PacketType>(result.Buffer);

            if (bufferDeserialized == desiredType)
            {
                SendPacketType(PacketType.PT_ACK, null, endpoint); // TODO: refactor all the shit outta here
                return true;
            }
            return false;
        }

        async private static void SendPacketType(PacketType request, PacketType? desiredResponse,
            IPEndPoint endpoint)
        {
            byte[] payload = JsonSerializer.SerializeToUtf8Bytes<PacketType>(request);
            int timeout = 1500;
            using var cst = new CancellationTokenSource(timeout);

            await Client.SendAsync(payload, payload.Length, endpoint);

            try
            {
                var result = await Client.ReceiveAsync();
                PacketType pt = JsonSerializer.Deserialize<PacketType>(result.Buffer);
                if (desiredResponse != null && pt != desiredResponse)
                {
                    throw new InvalidOperationException();
                }

                if (endpoint.Address.ToString().Equals(BroadcastAddress))
                {
                    RemoteConnectionPoint = result.RemoteEndPoint;
                }
            }
            catch (OperationCanceledException)
            {
                SendPacketType(request, desiredResponse, endpoint);
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }

        private static bool SendPacketTypeSync(PacketType request, PacketType desiredResponse,
            IPEndPoint endpoint)
        {
            byte[] payload = JsonSerializer.SerializeToUtf8Bytes<PacketType>(request);
            int timeout = 1500;

            Client.Send(payload, payload.Length, endpoint);

            var cts = new CancellationTokenSource(timeout);
            using Task task = Task.Run(async () =>
            {
                // byte[] result = Client.Receive(ref endpoint);
                var result = await Client.ReceiveAsync();

                //PacketType pt = JsonSerializer.Deserialize<PacketType>(result);
                PacketType pt = JsonSerializer.Deserialize<PacketType>(result.Buffer);
                if (cts.IsCancellationRequested)
                {
                    cts.Token.ThrowIfCancellationRequested();
                }

                if (pt != desiredResponse)
                {
                    throw new InvalidOperationException();
                }
            }, cts.Token);

            try
            {
                task.Wait(cts.Token);
                return true;
            }
            catch (OperationCanceledException)
            {
                cts.Cancel();
                // SendPacketTypeSync(request, desiredResponse, endpoint);
                // todo: display error message!
                return false;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
        }
    }
}
