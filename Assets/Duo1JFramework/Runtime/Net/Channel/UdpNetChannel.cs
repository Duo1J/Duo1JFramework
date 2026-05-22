using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace Duo1JFramework.Net
{
    /// <summary>
    /// UDP网络通道
    /// </summary>
    public class UdpNetChannel : INetChannel
    {
        private UdpClient client;
        private IPEndPoint remoteEndPoint;
        private Thread receiveThread;
        private volatile bool running;

        public string ChannelId { get; private set; }

        public ENetProtocol Protocol => ENetProtocol.UDP;

        public bool IsConnected => client != null && remoteEndPoint != null;

        public event Action<INetChannel, byte[]> OnReceive;

        public event Action<INetChannel, Exception> OnError;

        public UdpNetChannel(string channelId)
        {
            ChannelId = channelId;
        }

        public void Connect(string host, int port)
        {
            Close();

            IPAddress[] addresses = Dns.GetHostAddresses(host);
            if (addresses == null || addresses.Length <= 0)
            {
                throw new SocketException((int)SocketError.HostNotFound);
            }

            IPAddress address = addresses[0];
            client = new UdpClient(address.AddressFamily);
            remoteEndPoint = new IPEndPoint(address, port);
            client.Connect(remoteEndPoint);

            running = true;
            receiveThread = new Thread(ReceiveLoop);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        public void Send(byte[] bytes)
        {
            if (!IsConnected || bytes == null)
            {
                return;
            }

            client.Send(bytes, bytes.Length);
        }

        public void Close()
        {
            running = false;

            try
            {
                client?.Close();
            }
            catch (Exception e)
            {
                OnError?.Invoke(this, e);
            }
            finally
            {
                client = null;
                remoteEndPoint = null;
            }
        }

        public void Dispose()
        {
            Close();
        }

        private void ReceiveLoop()
        {
            try
            {
                while (running && client != null)
                {
                    IPEndPoint any = new IPEndPoint(IPAddress.Any, 0);
                    byte[] bytes = client.Receive(ref any);
                    if (bytes != null && bytes.Length > 0)
                    {
                        OnReceive?.Invoke(this, bytes);
                    }
                }
            }
            catch (Exception e)
            {
                if (running)
                {
                    OnError?.Invoke(this, e);
                }
            }
        }
    }
}
