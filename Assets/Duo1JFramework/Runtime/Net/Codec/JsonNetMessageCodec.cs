using System.Text;

namespace Duo1JFramework.Net
{
    /// <summary>
    /// Json网络消息编解码器
    /// </summary>
    public class JsonNetMessageCodec : INetMessageCodec
    {
        public ENetMessageFormat Format => ENetMessageFormat.Json;

        public byte[] Encode<T>(T message)
        {
            string json = JsonUtil.ToJson(message);
            return Encoding.UTF8.GetBytes(json ?? string.Empty);
        }

        public T Decode<T>(byte[] bytes)
        {
            if (bytes == null || bytes.Length <= 0)
            {
                return default(T);
            }

            string json = Encoding.UTF8.GetString(bytes);
            return JsonUtil.ToObject<T>(json);
        }
    }
}
