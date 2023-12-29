using System;
using System.Collections.Generic;

namespace Duo1JFramework.TimerUpdate
{
    /// <summary>
    /// Mono-Update更新管理器
    /// </summary>
    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        #region Update

        /// <summary>
        /// Update集合
        /// </summary>
        private HashSet<Action> updateSet;
        /// <summary>
        /// Update待移除列表
        /// </summary>
        private List<Action> updateDeleteList;

        /// <summary>
        /// 注册Update
        /// </summary>
        public void RegisterUpdate(Action updater)
        {
            if (updateSet.Contains(updater))
            {
                Log.ErrorForce("重复注册Update");
                return;
            }
            updateSet.Add(updater);
        }

        /// <summary>
        /// 取消注册Update
        /// </summary>
        public void UnRegisterUpdate(Action updater)
        {
            updateDeleteList.Add(updater);
        }

        private void Update()
        {
            if (updateSet != null)
            {
                if (updateDeleteList != null)
                {
                    foreach (Action action in updateDeleteList)
                    {
                        updateSet.Remove(action);
                    }
                    updateDeleteList.Clear();
                }
                foreach (Action action in updateSet)
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Assert.ExceptHandle(e);
                        UnRegisterUpdate(action);
                    }
                }
            }
        }

        #endregion Update

        #region LateUpdate

        /// <summary>
        /// LateUpdate集合
        /// </summary>
        private HashSet<Action> lateUpdateSet;
        /// <summary>
        /// LateUpdate待移除列表
        /// </summary>
        private List<Action> lateUpdateDeleteList;

        /// <summary>
        /// 注册LateUpdate
        /// </summary>
        public void RegisterLateUpdate(Action updater)
        {
            if (lateUpdateSet.Contains(updater))
            {
                Log.ErrorForce("重复注册LateUpdate");
                return;
            }
            lateUpdateSet.Add(updater);
        }

        /// <summary>
        /// 取消注册LateUpdate
        /// </summary>
        public void UnRegisterLateUpdate(Action updater)
        {
            lateUpdateDeleteList.Add(updater);
        }

        private void LateUpdate()
        {
            if (lateUpdateSet != null)
            {
                if (lateUpdateDeleteList != null)
                {
                    foreach (Action action in lateUpdateDeleteList)
                    {
                        lateUpdateSet.Remove(action);
                    }
                    lateUpdateDeleteList.Clear();
                }
                foreach (Action action in lateUpdateSet)
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Assert.ExceptHandle(e);
                        UnRegisterLateUpdate(action);
                    }
                }
            }
        }

        #endregion LateUpdate

        #region FixedUpdate

        /// <summary>
        /// FixedUpdate集合
        /// </summary>
        private HashSet<Action> fixedUpdateSet;
        /// <summary>
        /// FixedUpdate待移除列表
        /// </summary>
        private List<Action> fixedUpdateDeleteList;

        /// <summary>
        /// 注册FixedUpdate
        /// </summary>
        public void RegisterFixedUpdate(Action updater)
        {
            if (fixedUpdateSet.Contains(updater))
            {
                Log.ErrorForce("重复注册FixedUpdate");
                return;
            }
            fixedUpdateSet.Add(updater);
        }

        /// <summary>
        /// 取消注册FixedUpdate
        /// </summary>
        public void UnRegisterFixedUpdate(Action updater)
        {
            fixedUpdateDeleteList.Add(updater);
        }

        private void FixedUpdate()
        {
            if (fixedUpdateSet != null)
            {
                if (fixedUpdateDeleteList != null)
                {
                    foreach (Action action in fixedUpdateDeleteList)
                    {
                        fixedUpdateSet.Remove(action);
                    }
                    fixedUpdateDeleteList.Clear();
                }
                foreach (Action action in fixedUpdateSet)
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Assert.ExceptHandle(e);
                        UnRegisterFixedUpdate(action);
                    }
                }
            }
        }

        #endregion FixedUpdate

        protected override void OnInit()
        {
            updateSet = new HashSet<Action>();
            updateDeleteList = new List<Action>();
            fixedUpdateSet = new HashSet<Action>();
            fixedUpdateDeleteList = new List<Action>();
            lateUpdateSet = new HashSet<Action>();
            lateUpdateDeleteList = new List<Action>();
        }

        protected override void OnDispose()
        {
            updateSet = null;
            updateDeleteList = null;
            fixedUpdateSet = null;
            fixedUpdateDeleteList = null;
            lateUpdateSet = null;
            lateUpdateDeleteList = null;
        }
    }
}