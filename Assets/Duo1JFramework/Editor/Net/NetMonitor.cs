using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Net
{
    /// <summary>
    /// 网络消息监视器
    /// </summary>
    public class NetMonitor : BaseEditorWindow<NetMonitor>
    {
        private const int MAX_MESSAGE_COUNT = 200;
        private const int MAX_BODY_PREVIEW_LENGTH = 300;

        private static readonly Dictionary<ENetMessageFormat, INetMessageCodec> codecMap = new Dictionary<ENetMessageFormat, INetMessageCodec>()
        {
            [ENetMessageFormat.Json] = new JsonNetMessageCodec(),
            [ENetMessageFormat.Protobuf] = new ProtobufNetMessageCodec(),
        };

        private readonly List<NetMonitorItem> itemList = new List<NetMonitorItem>();

        private Vector2 scrollPos;
        private bool update = true;
        private bool registered;

        private void OnGUI()
        {
            if (!ED.IsPlayingHelpBox())
            {
                return;
            }

            TryRegisterNetEvent();

            ED.Horizontal(() =>
            {
                ED.Toggle(ref update, "每帧更新");

                if (GUILayout.Button("清空", GUILayout.Width(60)))
                {
                    itemList.Clear();
                }
            });

            ED.Scroll(ref scrollPos, () =>
            {
                GUILayout.Label($"网络消息列表：{itemList.Count}");

                for (int i = itemList.Count - 1; i >= 0; --i)
                {
                    DrawItem(itemList[i]);
                }
            });
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            UnRegisterNetEvent();
        }

        private void OnInspectorUpdate()
        {
            if (update)
            {
                Repaint();
            }
        }

        private void TryRegisterNetEvent()
        {
            if (registered)
            {
                return;
            }

            if (!NetManager.TryGetInstance(out NetManager netManager))
            {
                return;
            }

            netManager.OnMessageSent += OnMessageSent;
            netManager.OnMessageReceived += OnMessageReceived;

            registered = true;
        }

        private void UnRegisterNetEvent()
        {
            if (!registered)
            {
                return;
            }

            if (NetManager.TryGetInstance(out NetManager netManager))
            {
                netManager.OnMessageSent -= OnMessageSent;
                netManager.OnMessageReceived -= OnMessageReceived;
            }

            registered = false;
        }

        private void OnMessageSent(NetMessage message)
        {
            AddItem("发送", message);
        }

        private void OnMessageReceived(NetMessage message)
        {
            AddItem("接收", message);
        }

        private void AddItem(string direction, NetMessage message)
        {
            if (message == null)
            {
                return;
            }

            itemList.Add(new NetMonitorItem(direction, message));

            if (itemList.Count > MAX_MESSAGE_COUNT)
            {
                itemList.RemoveAt(0);
            }
        }

        private void DrawItem(NetMonitorItem item)
        {
            ED.Vertical(() =>
            {
                GUILayout.Label($"[{item.Time}] {item.Direction}");
                GUILayout.Label($"通道：{item.ChannelId}");
                GUILayout.Label($"协议：{item.Protocol}    格式：{item.Format}    字节：{item.ByteCount}");
                GUILayout.Label($"内容：{item.BodyPreview}");
            }, "box");
        }

        private class NetMonitorItem
        {
            public string Time { get; private set; }
            public string Direction { get; private set; }
            public string ChannelId { get; private set; }
            public ENetProtocol Protocol { get; private set; }
            public ENetMessageFormat Format { get; private set; }
            public int ByteCount { get; private set; }
            public string BodyPreview { get; private set; }

            public NetMonitorItem(string direction, NetMessage message)
            {
                Time = DateTime.Now.ToString("HH:mm:ss.fff");
                Direction = direction;
                ChannelId = message.ChannelId;
                Protocol = message.Protocol;
                Format = message.Format;
                ByteCount = message.Body == null ? 0 : message.Body.Length;
                BodyPreview = GetBodyPreview(message);
            }

            private static string GetBodyPreview(NetMessage message)
            {
                if (message.Body == null || message.Body.Length <= 0)
                {
                    return string.Empty;
                }

                if (!codecMap.TryGetValue(message.Format, out INetMessageCodec codec))
                {
                    return $"未注册网络消息编解码器：{message.Format}";
                }

                try
                {
                    object body = codec.Decode<object>(message.Body);
                    return LimitPreview(JsonUtil.ToJson(body));
                }
                catch (Exception e)
                {
                    return LimitPreview(e.Message);
                }
            }

            private static string LimitPreview(string preview)
            {
                if (string.IsNullOrEmpty(preview))
                {
                    return string.Empty;
                }

                if (preview.Length <= MAX_BODY_PREVIEW_LENGTH)
                {
                    return preview;
                }

                return preview.Substring(0, MAX_BODY_PREVIEW_LENGTH) + "...";
            }
        }
    }
}
