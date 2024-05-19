using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.TimerUpdate
{
    /// <summary>
    /// Mono-Update更新管理器
    /// </summary>
    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        #region Update

        /// <summary>
        /// PreUpdate集合
        /// </summary>
        private HashSet<Action> preUpdateSet;
        /// <summary>
        /// PreUpdate待添加列表
        /// </summary>
        private List<Action> preUpdateAddList;
        /// <summary>
        /// PreUpdate待移除列表
        /// </summary>
        private List<Action> preUpdateDeleteList;

        /// <summary>
        /// 注册PreUpdate
        /// </summary>
        public void RegisterPreUpdate(Action updater)
        {
            if ((preUpdateSet.Contains(updater) || preUpdateAddList.Contains(updater)) && !preUpdateDeleteList.Contains(updater))
            {
                Log.ErrorForce("重复注册PreUpdate");
                return;
            }
            preUpdateAddList.Add(updater);
        }

        /// <summary>
        /// 取消注册PreUpdate
        /// </summary>
        public void UnRegisterPreUpdate(Action updater)
        {
            preUpdateAddList.Remove(updater);
            preUpdateDeleteList.Add(updater);
        }

        /// <summary>
        /// Update集合
        /// </summary>
        private HashSet<Action> updateSet;
        /// <summary>
        /// Update待添加列表
        /// </summary>
        private List<Action> updateAddList;
        /// <summary>
        /// Update待移除列表
        /// </summary>
        private List<Action> updateDeleteList;

        /// <summary>
        /// 注册Update
        /// </summary>
        public void RegisterUpdate(Action updater)
        {
            if ((updateSet.Contains(updater) || updateAddList.Contains(updater)) && !updateDeleteList.Contains(updater))
            {
                Log.ErrorForce("重复注册Update");
                return;
            }
            updateAddList.Add(updater);
        }

        /// <summary>
        /// 取消注册Update
        /// </summary>
        public void UnRegisterUpdate(Action updater)
        {
            updateAddList.Remove(updater);
            updateDeleteList.Add(updater);
        }

        private void Update()
        {
            if (delayOneFrameSet != null)
            {
                foreach (Action call in delayOneFrameSet)
                {
                    call();
                }

                delayOneFrameSet.Clear();
            }

            //预先更新
            if (preUpdateSet != null)
            {
                if (preUpdateDeleteList != null)
                {
                    foreach (Action action in preUpdateDeleteList)
                    {
                        preUpdateSet.Remove(action);
                    }
                    preUpdateDeleteList.Clear();
                }
                if (preUpdateAddList != null)
                {
                    foreach (Action action in preUpdateAddList)
                    {
                        preUpdateSet.Add(action);
                    }
                    preUpdateAddList.Clear();
                }
                foreach (Action action in preUpdateSet)
                {
                    try
                    {
                        action?.Invoke();
                    }
                    catch (Exception e)
                    {
                        Assert.ExceptHandle(e);
                    }
                }
            }

            //更新
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
                if (updateAddList != null)
                {
                    foreach (Action action in updateAddList)
                    {
                        updateSet.Add(action);
                    }
                    updateAddList.Clear();
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
                    }
                }
            }

            //异步操作注册回调
            if (asyncOpeWrapList != null)
            {
                List<AsyncOperationWrap> removeList = null;
                foreach (AsyncOperationWrap wrap in asyncOpeWrapList)
                {
                    if (wrap.IsDone)
                    {
                        wrap.Call();

                        if (removeList == null)
                        {
                            removeList = new List<AsyncOperationWrap>();
                        }
                        removeList.Add(wrap);
                    }
                }
                if (removeList != null)
                {
                    foreach (AsyncOperationWrap wrap in removeList)
                    {
                        asyncOpeWrapList.Remove(wrap);
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
        /// LateUpdate待添加列表
        /// </summary>
        private List<Action> lateUpdateAddList;
        /// <summary>
        /// LateUpdate待移除列表
        /// </summary>
        private List<Action> lateUpdateDeleteList;

        /// <summary>
        /// 注册LateUpdate
        /// </summary>
        public void RegisterLateUpdate(Action updater)
        {
            if ((lateUpdateSet.Contains(updater) || lateUpdateAddList.Contains(updater)) && !lateUpdateDeleteList.Contains(updater))
            {
                Log.ErrorForce("重复注册LateUpdate");
                return;
            }
            lateUpdateAddList.Add(updater);
        }

        /// <summary>
        /// 取消注册LateUpdate
        /// </summary>
        public void UnRegisterLateUpdate(Action updater)
        {
            lateUpdateAddList.Remove(updater);
            lateUpdateDeleteList.Add(updater);
        }

        private void LateUpdate()
        {
            //延迟更新
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
                if (lateUpdateAddList != null)
                {
                    foreach (Action action in lateUpdateAddList)
                    {
                        lateUpdateSet.Add(action);
                    }
                    lateUpdateAddList.Clear();
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
        /// FixedUpdate待添加列表
        /// </summary>
        private List<Action> fixedUpdateAddList;
        /// <summary>
        /// FixedUpdate待移除列表
        /// </summary>
        private List<Action> fixedUpdateDeleteList;

        /// <summary>
        /// 注册FixedUpdate
        /// </summary>
        public void RegisterFixedUpdate(Action updater)
        {
            if ((fixedUpdateSet.Contains(updater) || fixedUpdateAddList.Contains(updater)) && !fixedUpdateDeleteList.Contains(updater))
            {
                Log.ErrorForce("重复注册FixedUpdate");
                return;
            }
            fixedUpdateAddList.Add(updater);
        }

        /// <summary>
        /// 取消注册FixedUpdate
        /// </summary>
        public void UnRegisterFixedUpdate(Action updater)
        {
            fixedUpdateAddList.Remove(updater);
            fixedUpdateDeleteList.Add(updater);
        }

        private void FixedUpdate()
        {
            //固定更新
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
                if (fixedUpdateAddList != null)
                {
                    foreach (Action action in fixedUpdateAddList)
                    {
                        fixedUpdateSet.Add(action);
                    }
                    fixedUpdateAddList.Clear();
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
                    }
                }
            }
        }

        #endregion FixedUpdate

        #region Delay

        private HashSet<Action> delayOneFrameSet;

        public void DelayOneFrame(Action action)
        {
            delayOneFrameSet.Add(action);
        }

        #endregion Delay

        #region Yield Request

        private List<AsyncOperationWrap> asyncOpeWrapList;

        public void RegisterAsyncRequest(AsyncOperation operation, Action<AsyncOperation> callback)
        {
            Assert.NotNull(operation, "AsyncOperation不可为空");
            Assert.NotNull(callback, "回调不可为空");

            operation.completed += (req) =>
            {
                callback?.Invoke(req);
            };

            //asyncOpeWrapList.Add(new AsyncOperationWrap(operation, callback));
        }

        #endregion Yield Request

        protected override void OnInit()
        {
            preUpdateSet = new HashSet<Action>();
            preUpdateAddList = new List<Action>();
            preUpdateDeleteList = new List<Action>();

            updateSet = new HashSet<Action>();
            updateAddList = new List<Action>();
            updateDeleteList = new List<Action>();

            fixedUpdateSet = new HashSet<Action>();
            fixedUpdateAddList = new List<Action>();
            fixedUpdateDeleteList = new List<Action>();

            lateUpdateSet = new HashSet<Action>();
            lateUpdateAddList = new List<Action>();
            lateUpdateDeleteList = new List<Action>();

            delayOneFrameSet = new HashSet<Action>();
            asyncOpeWrapList = new List<AsyncOperationWrap>();
        }

        protected override void OnDispose()
        {
            preUpdateSet = null;
            preUpdateAddList = null;
            preUpdateDeleteList = null;

            updateSet = null;
            updateAddList = null;
            updateDeleteList = null;

            fixedUpdateSet = null;
            fixedUpdateAddList = null;
            fixedUpdateDeleteList = null;

            lateUpdateSet = null;
            lateUpdateAddList = null;
            lateUpdateDeleteList = null;

            delayOneFrameSet = null;
            asyncOpeWrapList = null;
        }

        /// <summary>
        /// 异步操作回调包装
        /// </summary>
        private struct AsyncOperationWrap
        {
            private AsyncOperation operation;
            private Action<AsyncOperation> callback;

            public AsyncOperationWrap(AsyncOperation operation, Action<AsyncOperation> callback)
            {
                this.operation = operation;
                this.callback = callback;
            }

            public bool IsDone => operation.isDone;

            public void Call()
            {
                callback?.Invoke(operation);
                callback = null;
            }
        }
    }
}