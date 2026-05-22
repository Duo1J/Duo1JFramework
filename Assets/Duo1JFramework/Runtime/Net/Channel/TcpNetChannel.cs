using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Duo1JFramework.Net
{
    /// <summary>
    /// TCP网络通道
    /// </summary>
    public class TcpNetChannel : INetChannel
    {
        private TcpClient client;
        private NetworkStream stream;
        private Thread receiveThread;
        private volatile bool running;

        public string ChannelId { get; private set; }

        public ENetProtocol Protocol => ENetProtocol.TCP;

        public bool IsConnected => client != null && client.Connected;

        public event Action<INetChannel, byte[]> OnReceive;

        public event Action<INetChannel, Exception> OnError;

        public TcpNetChannel(string channelId)
        {
            ChannelId = channelId;
        }

        public void Connect(string host, int port)
        {
            Close();

            client = new TcpClient();
            client.NoDelay = true;
            client.Connect(host, port);
            stream = client.GetStream();

            running = true;
            receiveThread = new Thread(ReceiveLoop);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }

        public void Send(byte[] bytes)
        {
            if (!IsConnected || stream == null || bytes == null)
            {
                return;
            }

            byte[] lengthBytes = BitConverter.GetBytes(bytes.Length);
            stream.Write(lengthBytes, 0, lengthBytes.Length);
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }

        public void Close()
        {
            running = false;

            try
            {
                stream?.Close();
                client?.Close();
            }
            catch (Exception e)
            {
                OnError?.Invoke(this, e);
            }
            finally
            {
                stream = null;
                client = null;
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
                while (running && stream != null)
                {
                    byte[] lengthBytes = ReadExactly(sizeof(int));
                    if (lengthBytes == null)
                    {
                        break;
                    }

                    int length = BitConverter.ToInt32(lengthBytes, 0);
                    if (length <= 0)
                    {
                        continue;
                    }

                    byte[] body = ReadExactly(length);
                    if (body == null)
                    {
                        break;
                    }

                    OnReceive?.Invoke(this, body);
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

        private byte[] ReadExactly(int length)
        {
            byte[] buffer = new byte[length];
            int offset = 0;
            while (running && offset < length)
            {
                int read = stream.Read(buffer, offset, length - offset);
                if (read <= 0)
                {
                    return null;
                }

                offset += read;
            }

            return offset == length ? buffer : null;
        }
    }
}
