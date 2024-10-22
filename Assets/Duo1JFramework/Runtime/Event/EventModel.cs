using System;
using System.Collections.Generic;

namespace Duo1JFramework.Event
{
    /// <summary>
    /// 事件模型
    /// </summary>
    public class EventModel : IEventModel
    {
        private Dictionary<object, List<Action<object>>> eventDict;

        public void Register(object e, Action<object> callback)
        {
            if (!eventDict.TryGetValue(e, out List<Action<object>> list))
            {
                list = new List<Action<object>>();
                eventDict.Add(e, list);
            }
            list.Add(callback);
        }

        public bool UnRegister(object e, Action<object> callback)
        {
            if (!eventDict.TryGetValue(e, out List<Action<object>> list))
            {
                throw Except.Create($"事件`{e.ToString()}`未找到任何订阅，无法取消订阅");
            }
            return list.Remove(callback);
        }

        public bool UnRegister(object e)
        {
            return eventDict.Remove(e);
        }

        public void UnRegisterAll()
        {
            eventDict.Clear();
        }

        public void Broadcast(object e, object args = null)
        {
            if (eventDict.TryGetValue(e, out List<Action<object>> list))
            {
                list.ForEach(action => action(args));
            }
        }

        public EventModel()
        {
            eventDict = new Dictionary<object, List<Action<object>>>();
        }
    }
}
