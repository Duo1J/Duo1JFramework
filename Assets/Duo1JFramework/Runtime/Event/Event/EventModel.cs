using System;
using System.Collections.Generic;

namespace Duo1JFramework.Event
{
    /// <summary>
    /// 事件模型
    /// </summary>
    public class EventModel : IEventModel
    {
        /// <summary>
        /// 事件字典
        /// </summary>
        private Dictionary<object, HashSet<Action<object>>> eventDict = null;

        /// <summary>
        /// 订阅事件
        /// </summary>
        public void Register(object e, Action<object> callback)
        {
            Assert.NotNullArg(e, "e");
            Assert.NotNullArg(callback, "callback");

            if (!eventDict.TryGetValue(e, out HashSet<Action<object>> set))
            {
                set = new HashSet<Action<object>>();
                eventDict.Add(e, set);
            }

            set.Add(callback);
        }

        /// <summary>
        /// 取消订阅事件
        /// </summary>
        public bool UnRegister(object e, Action<object> callback)
        {
            Assert.NotNullArg(e, "e");
            Assert.NotNullArg(callback, "callback");

            if (!eventDict.TryGetValue(e, out HashSet<Action<object>> set))
            {
                Log.ErrorForce($"事件 `{e.ToString()}` 未找到任何订阅，无法取消");
                return false;
            }

            return set.Remove(callback);
        }

        /// <summary>
        /// 取消订阅事件下所有注册
        /// </summary>
        public bool UnRegister(object e)
        {
            Assert.NotNullArg(e, "e");

            return eventDict.Remove(e);
        }

        /// <summary>
        /// 取消订阅所有事件
        /// </summary>
        public void UnRegisterAll()
        {
            eventDict.Clear();
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        public void Broadcast(object e, object args = null)
        {
            Assert.NotNullArg(e, "e");

            if (eventDict.TryGetValue(e, out HashSet<Action<object>> set))
            {
                if (set.Count > 0)
                {
                    Action<object>[] actionArr = new Action<object>[set.Count];
                    set.CopyTo(actionArr);

                    foreach (Action<object> action in actionArr)
                    {
                        if (action == null)
                        {
                            continue;
                        }

                        action(e);
                    }
                }
            }
        }

        public EventModel()
        {
            eventDict = new Dictionary<object, HashSet<Action<object>>>();
        }
    }
}
