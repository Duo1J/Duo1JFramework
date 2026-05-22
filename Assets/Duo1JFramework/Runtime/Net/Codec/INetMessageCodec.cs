namespace Duo1JFramework.Net
{
    /// <summary>
    /// 网络消息编解码器
    /// </summary>
    public interface INetMessageCodec
    {
        ENetMessageFormat Format { get; }

        byte[] Encode<T>(T message);

        T Decode<T>(byte[] bytes);
    }
}
