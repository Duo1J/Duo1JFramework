using System;
using System.Collections.Generic;

namespace Duo1JFramework.Scheduling
{
    /// <summary>
    /// 更新组
    /// </summary>
    internal class UpdateGroup
    {
        /// <summary>
        /// 更新组名称
        /// </summary>
        private string name;

        /// <summary>
        /// 更新集合
        /// </summary>
        private HashSet<Action> updateSet;

        /// <summary>
        /// 待添加列表
        /// </summary>
        private List<Action> addList;

        /// <summary>
        /// 待移除列表
        /// </summary>
        private List<Action> removeList;

        public UpdateGroup(string name)
        {
            this.name = name;
            updateSet = new HashSet<Action>();
            addList = new List<Action>();
            removeList = new List<Action>();
        }

        /// <summary>
        /// 注册更新
        /// </summary>
        public void Register(Action updater)
        {
            Assert.NotNullArg(updater, "updater");

            if ((updateSet.Contains(updater) || addList.Contains(updater)) && !removeList.Contains(updater))
            {
                Log.ErrorForce($"UpdateGroup重复注册{name}");
                return;
            }

            addList.Add(updater);
        }

        /// <summary>
        /// 取消注册更新
        /// </summary>
        public void UnRegister(Action updater)
        {
            Assert.NotNullArg(updater, "updater");

            addList.Remove(updater);

            if (!removeList.Contains(updater))
            {
                removeList.Add(updater);
            }
        }

        /// <summary>
        /// 执行更新
        /// </summary>
        public void Tick()
        {
            foreach (Action action in removeList)
            {
                updateSet.Remove(action);
            }

            removeList.Clear();

            foreach (Action action in addList)
            {
                updateSet.Add(action);
            }

            addList.Clear();

            foreach (Action action in updateSet)
            {
                try
                {
                    action?.Invoke();
                }
                catch (Exception e)
                {
                    Assert.ExceptHandle(e, $"UpdateGroup-{name} 更新异常");
                }
            }
        }
    }
}
