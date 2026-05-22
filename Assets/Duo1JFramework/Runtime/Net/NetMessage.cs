namespace Duo1JFramework.Net
{
    /// <summary>
    /// 网络消息
    /// </summary>
    public class NetMessage
    {
        public string ChannelId { get; private set; }

        public ENetProtocol Protocol { get; private set; }

        public ENetMessageFormat Format { get; private set; }

        public byte[] Body { get; private set; }

        public NetMessage(string channelId, ENetProtocol protocol, ENetMessageFormat format, byte[] body)
        {
            ChannelId = channelId;
            Protocol = protocol;
            Format = format;
            Body = body;
        }
    }
}
