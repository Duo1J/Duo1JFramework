using System;
using System.Collections.Generic;

namespace Duo1JFramework.Net
{
    /// <summary>
    /// 网络管理器
    /// </summary>
    public class NetManager : MonoSingleton<NetManager>
    {
        private readonly Dictionary<string, INetChannel> channels = new Dictionary<string, INetChannel>();
        private readonly Dictionary<string, ENetMessageFormat> channelFormats = new Dictionary<string, ENetMessageFormat>();
        private readonly Dictionary<ENetMessageFormat, INetMessageCodec> codecs = new Dictionary<ENetMessageFormat, INetMessageCodec>();

        private readonly Queue<NetMessage> receiveMessages = new Queue<NetMessage>();
        private readonly Queue<Exception> errors = new Queue<Exception>();

        private readonly object receiveLock = new object();
        private readonly object errorLock = new object();

        public event Action<NetMessage> OnMessageSent;
        public event Action<NetMessage> OnMessageReceived;
        public event Action<Exception> OnNetError;

        protected override void OnInit()
        {
            RegisterCodec(new JsonNetMessageCodec());
            RegisterCodec(new ProtobufNetMessageCodec());

            Reg.RegisterUpdate(OnNetUpdate);
        }

        protected override void OnDispose()
        {
            Reg.UnRegisterUpdate();
            CloseAll();
            codecs.Clear();
            ClearQueue();
        }

        private void OnNetUpdate()
        {
            DispatchMessages();
            DispatchErrors();
        }

        /// <summary>
        /// 注册消息编解码器
        /// </summary>
        public void RegisterCodec(INetMessageCodec codec)
        {
            if (codec == null)
            {
                return;
            }

            codecs[codec.Format] = codec;
        }

        /// <summary>
        /// 创建并连接TCP通道
        /// </summary>
        public INetChannel ConnectTcp(string channelId, string host, int port, ENetMessageFormat format = ENetMessageFormat.Json)
        {
            return Connect(channelId, ENetProtocol.TCP, host, port, format);
        }

        /// <summary>
        /// 创建并连接UDP通道
        /// </summary>
        public INetChannel ConnectUdp(string channelId, string host, int port, ENetMessageFormat format = ENetMessageFormat.Json)
        {
            return Connect(channelId, ENetProtocol.UDP, host, port, format);
        }

        /// <summary>
        /// 创建并连接网络通道
        /// </summary>
        public INetChannel Connect(string channelId, ENetProtocol protocol, string host, int port, ENetMessageFormat format = ENetMessageFormat.Json)
        {
            if (string.IsNullOrEmpty(channelId))
            {
                Log.Error("网络通道Id不能为空");
                return null;
            }

            Close(channelId);

            INetChannel channel = CreateChannel(channelId, protocol);
            channel.OnReceive += HandleChannelReceive;
            channel.OnError += HandleChannelError;
            channel.Connect(host, port);

            channels[channelId] = channel;
            channelFormats[channelId] = format;

            return channel;
        }

        /// <summary>
        /// 发送Json消息
        /// </summary>
        public void SendJson<T>(string channelId, T message)
        {
            Send(channelId, message, ENetMessageFormat.Json);
        }

        /// <summary>
        /// 发送Protobuf消息
        /// </summary>
        public void SendProtobuf<T>(string channelId, T message)
        {
            Send(channelId, message, ENetMessageFormat.Protobuf);
        }

        /// <summary>
        /// 发送指定格式消息
        /// </summary>
        public void Send<T>(string channelId, T message, ENetMessageFormat format)
        {
            if (!codecs.TryGetValue(format, out INetMessageCodec codec))
            {
                Log.Error($"未注册网络消息编解码器：{format}");
                return;
            }

            SendBytes(channelId, codec.Encode(message), format);
        }

        /// <summary>
        /// 发送原始字节消息
        /// </summary>
        public void SendBytes(string channelId, byte[] bytes)
        {
            ENetMessageFormat format = channelFormats.TryGetValue(channelId, out ENetMessageFormat value) ? value : ENetMessageFormat.Json;
            SendBytes(channelId, bytes, format);
        }

        private void SendBytes(string channelId, byte[] bytes, ENetMessageFormat format)
        {
            if (!channels.TryGetValue(channelId, out INetChannel channel))
            {
                Log.Error($"未找到网络通道：{channelId}");
                return;
            }

            channel.Send(bytes);
            OnMessageSent?.Invoke(new NetMessage(channelId, channel.Protocol, format, bytes));
        }

        /// <summary>
        /// 将网络消息解码为指定类型
        /// </summary>
        public T Decode<T>(NetMessage message)
        {
            if (message == null)
            {
                return default(T);
            }

            if (!codecs.TryGetValue(message.Format, out INetMessageCodec codec))
            {
                Log.Error($"未注册网络消息编解码器：{message.Format}");
                return default(T);
            }

            return codec.Decode<T>(message.Body);
        }

        /// <summary>
        /// 获取通道
        /// </summary>
        public bool TryGetChannel(string channelId, out INetChannel channel)
        {
            return channels.TryGetValue(channelId, out channel);
        }

        /// <summary>
        /// 关闭通道
        /// </summary>
        public void Close(string channelId)
        {
            if (!channels.TryGetValue(channelId, out INetChannel channel))
            {
                return;
            }

            channel.OnReceive -= HandleChannelReceive;
            channel.OnError -= HandleChannelError;
            channel.Close();
            channel.Dispose();
            channels.Remove(channelId);
            channelFormats.Remove(channelId);
        }

        /// <summary>
        /// 关闭所有通道
        /// </summary>
        public void CloseAll()
        {
            List<string> channelIds = new List<string>(channels.Keys);
            for (int i = 0; i < channelIds.Count; i++)
            {
                Close(channelIds[i]);
            }
        }

        private INetChannel CreateChannel(string channelId, ENetProtocol protocol)
        {
            switch (protocol)
            {
                case ENetProtocol.TCP:
                    return new TcpNetChannel(channelId);
                case ENetProtocol.UDP:
                    return new UdpNetChannel(channelId);
                default:
                    throw new ArgumentOutOfRangeException(nameof(protocol), protocol, null);
            }
        }

        private void HandleChannelReceive(INetChannel channel, byte[] bytes)
        {
            ENetMessageFormat format = channelFormats.TryGetValue(channel.ChannelId, out ENetMessageFormat value) ? value : ENetMessageFormat.Json;
            lock (receiveLock)
            {
                receiveMessages.Enqueue(new NetMessage(channel.ChannelId, channel.Protocol, format, bytes));
            }
        }

        private void HandleChannelError(INetChannel channel, Exception exception)
        {
            lock (errorLock)
            {
                errors.Enqueue(exception);
            }
        }

        private void DispatchMessages()
        {
            while (true)
            {
                NetMessage message;
                lock (receiveLock)
                {
                    if (receiveMessages.Count <= 0)
                    {
                        break;
                    }

                    message = receiveMessages.Dequeue();
                }

                OnMessageReceived?.Invoke(message);
            }
        }

        private void DispatchErrors()
        {
            while (true)
            {
                Exception exception;
                lock (errorLock)
                {
                    if (errors.Count <= 0)
                    {
                        break;
                    }

                    exception = errors.Dequeue();
                }

                OnNetError?.Invoke(exception);
            }
        }

        private void ClearQueue()
        {
            lock (receiveLock)
            {
                receiveMessages.Clear();
            }

            lock (errorLock)
            {
                errors.Clear();
            }
        }
    }
}
