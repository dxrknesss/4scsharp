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
        public static bool ConnectionEstablished;
        public static IPEndPoint RemoteConnectionPoint;
        readonly static int _timeout = 1000;

        public enum PacketType : byte
        {
            PT_HELLO, PT_ACK, PT_POINT, PT_READY, PT_CHNST, PT_RNDSD
        }

        public enum PeerType : byte
        {
            HOST, CLIENT
        }

        public static void SetClient(string ipAddr, int port)
        {
            IPEndPoint remotePoint, remotePoint2;
            try
            {
                remotePoint = new IPEndPoint(IPAddress.Parse(ipAddr), port);
                //remotePoint2 = new IPEndPoint(IPAddress.Parse(ipAddr), port+1);
            }
            catch (FormatException exception)
            {
                throw exception;
            }

            Client ??= new UdpClient() { EnableBroadcast = true };
            RemoteConnectionPoint = remotePoint;
        }

        public static void SetClient(int port)
        {
            Client ??= new UdpClient(port) { EnableBroadcast = true };
        }

        async public static Task<bool> ReceivePacketAsync(byte desiredHeader)
        {
            var result = await Client.ReceiveAsync();
            var endpoint = result.RemoteEndPoint;
            byte[] bufferDeserialized = result.Buffer;

            if (desiredHeader == bufferDeserialized[0])
            {
                await SendDataAsync(new byte[] { (byte)PacketType.PT_ACK }, null, endpoint);
                return true;
            }
            return false;
        }

        public static bool ReceivePacketSync(PacketType desiredHeader, out byte[] payload)
        {
            byte[] result = Client.Receive(ref RemoteConnectionPoint);
            PacketType deserializedHeader = (PacketType)result[0];
            payload = result;

            if (deserializedHeader == desiredHeader)
            {
                SendDataSync(new byte[] { (byte)PacketType.PT_ACK }, null, RemoteConnectionPoint);
                return true;
            }
            return false;
        }


        public static bool ReceivePacketSync(PacketType desiredHeader, out byte[] payload, bool sendAck)
        {
            byte[] result = Client.Receive(ref RemoteConnectionPoint);
            PacketType deserializedHeader = (PacketType)result[0];
            payload = result;

            if (deserializedHeader == desiredHeader)
            {
                if (sendAck)
                {
                    SendDataSync(new byte[] { (byte)PacketType.PT_ACK }, null, RemoteConnectionPoint);
                }
                return true;
            }
            return false;
        }

        async internal static Task<bool> SendDataAsync(byte[] payload, PacketType? desiredResponse,
            IPEndPoint endpoint)
        {
            await Client.SendAsync(payload, payload.Length, endpoint);

            if(desiredResponse == null)
            {
                return true;
            }

            try
            {
                var result = await Client.ReceiveAsync();
                PacketType pt = (PacketType)result.Buffer[0];
                if (pt != desiredResponse)
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

        internal static bool SendDataSync(byte[] payload, PacketType? desiredResponse,
            IPEndPoint endpoint)
        {
            Client.Send(payload, payload.Length, endpoint);

            if (desiredResponse == null)
            {
                return true;
            }

            try
            {
                Task<byte[]> receiveRes = Task.Run(() =>
                {
                    var result = Client.Receive(ref endpoint);
                    return result;
                });

                Task.WaitAny(new Task[] { receiveRes, Task.Delay(_timeout) });

                if (!receiveRes.IsCompleted)
                {
                    throw new OperationCanceledException();
                }
                PacketType pt = (PacketType)receiveRes.Result[0];

                if (pt != desiredResponse)
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
    }
}
