using Duo1JFramework.Asset;
using System;
using UnityEngine;

namespace Duo1JFramework.Timeline
{
    /// <summary>
    /// Timeline管理器
    /// </summary>
    public class TimelineManager : MonoSingleton<TimelineManager>
    {
        /// <summary>
        /// 异步加载Timeline
        /// </summary>
        public void LoadTimeline(string timelinePath, Action<TimelineData> callback = null)
        {
            AssetManager.Instance.LoadIns<GameObject>(timelinePath, (go) =>
            {
                TimelineData td = WrapTimelinePrefab(go);
                callback?.Invoke(td);
            });
        }

        /// <summary>
        /// 同步加载Timeline
        /// </summary>
        public TimelineData LoadTimelineSync(string timelinePath)
        {
            GameObject go = AssetManager.Instance.LoadInsSync<GameObject>(timelinePath);
            return WrapTimelinePrefab(go);
        }

        /// <summary>
        /// 创建Timeline包装类
        /// </summary>
        private TimelineData WrapTimelinePrefab(GameObject go)
        {
            try
            {
                return new TimelineData(go);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
                if (go != null)
                {
                    go.DestroyImmediate();
                }
                return null;
            }
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
        }
    }
}