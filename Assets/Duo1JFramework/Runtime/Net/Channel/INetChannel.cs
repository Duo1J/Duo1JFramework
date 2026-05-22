using System;

namespace Duo1JFramework.Net
{
    /// <summary>
    /// 网络通道
    /// </summary>
    public interface INetChannel : IDisposable
    {
        string ChannelId { get; }

        ENetProtocol Protocol { get; }

        bool IsConnected { get; }

        event Action<INetChannel, byte[]> OnReceive;

        event Action<INetChannel, Exception> OnError;

        void Connect(string host, int port);

        void Send(byte[] bytes);

        void Close();
    }
}
