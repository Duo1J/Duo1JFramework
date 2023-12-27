using System;
using System.Collections.Generic;

namespace Duo1JFramework.TimerUpdate
{
    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        /// <summary>
        /// 更新集合
        /// </summary>
        private HashSet<Action> updateSet;

        /// <summary>
        /// 注册Update
        /// </summary>
        public void Register(Action updater)
        {
            if (updateSet.Contains(updater))
            {
                Log.Warn("重复注册Update");
                return;
            }
            updateSet.Add(updater);
        }

        /// <summary>
        /// 取消注册Update
        /// </summary>
        public void UnRegister(Action updater)
        {
            updateSet.Remove(updater);
        }

        private void Update()
        {
            if (updateSet != null)
            {
                foreach (Action action in updateSet)
                {
                    action?.Invoke();
                }
            }
        }

        protected override void OnDispose()
        {
            updateSet = null;
        }

        protected override void OnInit()
        {
            updateSet = new HashSet<Action>();
        }
    }
}