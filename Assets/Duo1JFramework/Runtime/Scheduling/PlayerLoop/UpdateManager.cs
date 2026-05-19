using System;
using System.Collections.Generic;
using UnityEngine;

//////
/// Update顺序
/// EarlyUpdate -> PreUpdate -> Update -> LateUpdate
//////

namespace Duo1JFramework.Scheduling
{
    /// <summary>
    /// Mono Update更新管理器
    /// </summary>
    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        #region EarlyUpdate

        /// <summary>
        /// EarlyUpdate集合
        /// </summary>
        private HashSet<Action> earlyUpdateSet;

        /// <summary>
        /// EarlyUpdate待添加列表
        /// </summary>
        private List<Action> earlyUpdateAddList;

        /// <summary>
        /// EarlyUpdate待移除列表
        /// </summary>
        private List<Action> earlyUpdateRemoveList;

        /// <summary>
        /// 注册EarlyUpdate
        /// </summary>
        public void RegisterEarlyUpdate(Action updater)
        {
            if ((earlyUpdateSet.Contains(updater) || earlyUpdateAddList.Contains(updater)) && !earlyUpdateRemoveList.Contains(updater))
            {
                Log.ErrorForce("重复注册EarlyUpdate");
                return;
            }

            earlyUpdateAddList.Add(updater);
        }

        /// <summary>
        /// 取消注册EarlyUpdate
        /// </summary>
        public void UnRegisterEarlyUpdate(Action updater)
        {
            earlyUpdateAddList.Remove(updater);
            earlyUpdateRemoveList.Add(updater);
        }

        private void OnEarlyUpdate()
        {
            if (earlyUpdateSet != null)
            {
                if (earlyUpdateRemoveList != null)
                {
                    foreach (Action action in earlyUpdateRemoveList)
                    {
                        earlyUpdateSet.Remove(action);
                    }

                    earlyUpdateRemoveList.Clear();
                }

                if (earlyUpdateAddList != null)
                {
                    foreach (Action action in earlyUpdateAddList)
                    {
                        earlyUpdateSet.Add(action);
                    }

                    earlyUpdateAddList.Clear();
                }

                foreach (Action action in earlyUpdateSet)
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

        #endregion EarlyUpdate

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
        private List<Action> preUpdateRemoveList;

        /// <summary>
        /// 注册PreUpdate
        /// </summary>
        public void RegisterPreUpdate(Action updater)
        {
            if ((preUpdateSet.Contains(updater) || preUpdateAddList.Contains(updater)) && !preUpdateRemoveList.Contains(updater))
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
            preUpdateRemoveList.Add(updater);
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
        private List<Action> updateRemoveList;

        /// <summary>
        /// 注册Update
        /// </summary>
        public void RegisterUpdate(Action updater)
        {
            if ((updateSet.Contains(updater) || updateAddList.Contains(updater)) && !updateRemoveList.Contains(updater))
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
            updateRemoveList.Add(updater);
        }

        private void Update()
        {
            if (preUpdateSet != null)
            {
                if (preUpdateRemoveList != null)
                {
                    foreach (Action action in preUpdateRemoveList)
                    {
                        preUpdateSet.Remove(action);
                    }

                    preUpdateRemoveList.Clear();
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

            if (updateSet != null)
            {
                if (updateRemoveList != null)
                {
                    foreach (Action action in updateRemoveList)
                    {
                        updateSet.Remove(action);
                    }

                    updateRemoveList.Clear();
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

            if (asyncOpeWrapList != null)
            {
                List<AsyncOperationWrap> completedList = null;
                for (int i = asyncOpeWrapList.Count - 1; i >= 0; --i)
                {
                    AsyncOperationWrap wrap = asyncOpeWrapList[i];
                    if (!wrap.IsDone)
                    {
                        continue;
                    }

                    if (completedList == null)
                    {
                        completedList = new List<AsyncOperationWrap>();
                    }

                    completedList.Add(wrap);
                    asyncOpeWrapList.RemoveAt(i);
                }

                if (completedList != null)
                {
                    foreach (AsyncOperationWrap wrap in completedList)
                    {
                        wrap.Call();
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
        private List<Action> lateUpdateRemoveList;

        /// <summary>
        /// 注册LateUpdate
        /// </summary>
        public void RegisterLateUpdate(Action updater)
        {
            if ((lateUpdateSet.Contains(updater) || lateUpdateAddList.Contains(updater)) && !lateUpdateRemoveList.Contains(updater))
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
            lateUpdateRemoveList.Add(updater);
        }

        private void LateUpdate()
        {
            //延迟更新
            if (lateUpdateSet != null)
            {
                if (lateUpdateRemoveList != null)
                {
                    foreach (Action action in lateUpdateRemoveList)
                    {
                        lateUpdateSet.Remove(action);
                    }

                    lateUpdateRemoveList.Clear();
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
        private List<Action> fixedUpdateRemoveList;

        /// <summary>
        /// 注册FixedUpdate
        /// </summary>
        public void RegisterFixedUpdate(Action updater)
        {
            if ((fixedUpdateSet.Contains(updater) || fixedUpdateAddList.Contains(updater)) && !fixedUpdateRemoveList.Contains(updater))
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
            fixedUpdateRemoveList.Add(updater);
        }

        private void FixedUpdate()
        {
            if (fixedUpdateSet != null)
            {
                if (fixedUpdateRemoveList != null)
                {
                    foreach (Action action in fixedUpdateRemoveList)
                    {
                        fixedUpdateSet.Remove(action);
                    }

                    fixedUpdateRemoveList.Clear();
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

        #region Yield Request

        /// <summary>
        /// 异步操作回调包装列表
        /// </summary>
        private List<AsyncOperationWrap> asyncOpeWrapList;

        /// <summary>
        /// 注册异步操作回调
        /// </summary>
        public void RegisterAsyncRequest(AsyncOperation operation, Action<AsyncOperation> callback)
        {
            Assert.NotNullArg(operation, "operation");
            Assert.NotNullArg(callback, "callback");

            if (asyncOpeWrapList == null)
            {
                Log.Warn("UpdateManager已释放，忽略异步操作回调注册");
                return;
            }

            asyncOpeWrapList.Add(new AsyncOperationWrap(operation, callback));
        }

        #endregion Yield Request

        protected override void OnInit()
        {
            PlayerLoopManager.Instance.AddPlayerLoop(typeof(UnityEngine.PlayerLoop.EarlyUpdate), OnEarlyUpdate);

            earlyUpdateSet = new HashSet<Action>();
            earlyUpdateAddList = new List<Action>();
            earlyUpdateRemoveList = new List<Action>();

            preUpdateSet = new HashSet<Action>();
            preUpdateAddList = new List<Action>();
            preUpdateRemoveList = new List<Action>();

            updateSet = new HashSet<Action>();
            updateAddList = new List<Action>();
            updateRemoveList = new List<Action>();

            fixedUpdateSet = new HashSet<Action>();
            fixedUpdateAddList = new List<Action>();
            fixedUpdateRemoveList = new List<Action>();

            lateUpdateSet = new HashSet<Action>();
            lateUpdateAddList = new List<Action>();
            lateUpdateRemoveList = new List<Action>();

            asyncOpeWrapList = new List<AsyncOperationWrap>();
        }

        protected override void OnDispose()
        {
            earlyUpdateSet = null;
            earlyUpdateAddList = null;
            earlyUpdateRemoveList = null;

            preUpdateSet = null;
            preUpdateAddList = null;
            preUpdateRemoveList = null;

            updateSet = null;
            updateAddList = null;
            updateRemoveList = null;

            fixedUpdateSet = null;
            fixedUpdateAddList = null;
            fixedUpdateRemoveList = null;

            lateUpdateSet = null;
            lateUpdateAddList = null;
            lateUpdateRemoveList = null;

            asyncOpeWrapList = null;
        }

        /// <summary>
        /// 异步操作回调包装
        /// </summary>
        private struct AsyncOperationWrap : IEquatable<AsyncOperationWrap>
        {
            /// <summary>
            /// 异步操作
            /// </summary>
            private AsyncOperation operation;

            /// <summary>
            /// 异步操作完成回调
            /// </summary>
            private Action<AsyncOperation> callback;

            /// <summary>
            /// 是否完成
            /// </summary>
            public bool IsDone => operation == null || operation.isDone;

            public AsyncOperationWrap(AsyncOperation operation, Action<AsyncOperation> callback)
            {
                this.operation = operation;
                this.callback = callback;
            }

            public void Call()
            {
                try
                {
                    callback?.Invoke(operation);
                }
                catch (Exception e)
                {
                    Assert.ExceptHandle(e, "异步操作完成回调异常");
                }
                finally
                {
                    callback = null;
                    operation = null;
                }
            }

            public bool Equals(AsyncOperationWrap other)
            {
                return Equals(operation, other.operation) && Equals(callback, other.callback);
            }

            public override bool Equals(object obj)
            {
                return obj is AsyncOperationWrap other && Equals(other);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(operation, callback);
            }
        }
    }
}