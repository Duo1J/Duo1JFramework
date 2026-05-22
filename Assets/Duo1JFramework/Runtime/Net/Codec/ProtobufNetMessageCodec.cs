using System;

namespace Duo1JFramework.Net
{
    /// <summary>
    /// Protobuf网络消息编解码器
    /// </summary>
    public class ProtobufNetMessageCodec : INetMessageCodec
    {
        public ENetMessageFormat Format => ENetMessageFormat.Protobuf;

        public byte[] Encode<T>(T message)
        {
            throw new NotImplementedException("Protobuf消息编码暂未实现");
        }

        public T Decode<T>(byte[] bytes)
        {
            throw new NotImplementedException("Protobuf消息解码暂未实现");
        }
    }
}
