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
        /// <summary>
        /// EarlyUpdate更新组
        /// </summary>
        private UpdateGroup earlyUpdateGroup;

        /// <summary>
        /// PreUpdate更新组
        /// </summary>
        private UpdateGroup preUpdateGroup;

        /// <summary>
        /// Update更新组
        /// </summary>
        private UpdateGroup updateGroup;

        /// <summary>
        /// LateUpdate更新组
        /// </summary>
        private UpdateGroup lateUpdateGroup;

        /// <summary>
        /// FixedUpdate更新组
        /// </summary>
        private UpdateGroup fixedUpdateGroup;

        #region EarlyUpdate

        /// <summary>
        /// 注册EarlyUpdate
        /// </summary>
        public void RegisterEarlyUpdate(Action updater)
        {
            earlyUpdateGroup.Register(updater);
        }

        /// <summary>
        /// 取消注册EarlyUpdate
        /// </summary>
        public void UnRegisterEarlyUpdate(Action updater)
        {
            earlyUpdateGroup.UnRegister(updater);
        }

        private void OnEarlyUpdate()
        {
            earlyUpdateGroup.Tick();
        }

        #endregion EarlyUpdate

        #region Update

        /// <summary>
        /// 注册PreUpdate
        /// </summary>
        public void RegisterPreUpdate(Action updater)
        {
            preUpdateGroup.Register(updater);
        }

        /// <summary>
        /// 取消注册PreUpdate
        /// </summary>
        public void UnRegisterPreUpdate(Action updater)
        {
            preUpdateGroup.UnRegister(updater);
        }

        /// <summary>
        /// 注册Update
        /// </summary>
        public void RegisterUpdate(Action updater)
        {
            updateGroup.Register(updater);
        }

        /// <summary>
        /// 取消注册Update
        /// </summary>
        public void UnRegisterUpdate(Action updater)
        {
            updateGroup.UnRegister(updater);
        }

        private void Update()
        {
            preUpdateGroup.Tick();
            updateGroup.Tick();
            TickAsyncOperation();
        }

        #endregion Update

        #region LateUpdate

        /// <summary>
        /// 注册LateUpdate
        /// </summary>
        public void RegisterLateUpdate(Action updater)
        {
            lateUpdateGroup.Register(updater);
        }

        /// <summary>
        /// 取消注册LateUpdate
        /// </summary>
        public void UnRegisterLateUpdate(Action updater)
        {
            lateUpdateGroup.UnRegister(updater);
        }

        private void LateUpdate()
        {
            lateUpdateGroup.Tick();
        }

        #endregion LateUpdate

        #region FixedUpdate

        /// <summary>
        /// 注册FixedUpdate
        /// </summary>
        public void RegisterFixedUpdate(Action updater)
        {
            fixedUpdateGroup.Register(updater);
        }

        /// <summary>
        /// 取消注册FixedUpdate
        /// </summary>
        public void UnRegisterFixedUpdate(Action updater)
        {
            fixedUpdateGroup.UnRegister(updater);
        }

        private void FixedUpdate()
        {
            fixedUpdateGroup.Tick();
        }

        #endregion FixedUpdate

        #region Yield Request

        /// <summary>
        /// 异步操作回调包装列表
        /// </summary>
        private List<AsyncOperationWrap> asyncOpeWrapList;

        /// <summary>
        /// 异步操作完成回调缓存
        /// </summary>
        private List<AsyncOperationWrap> asyncOpeCompletedList;

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

        private void TickAsyncOperation()
        {
            if (asyncOpeWrapList == null)
            {
                return;
            }

            asyncOpeCompletedList.Clear();

            for (int i = asyncOpeWrapList.Count - 1; i >= 0; --i)
            {
                AsyncOperationWrap wrap = asyncOpeWrapList[i];
                if (!wrap.IsDone)
                {
                    continue;
                }

                asyncOpeCompletedList.Add(wrap);
                asyncOpeWrapList.RemoveAt(i);
            }

            foreach (AsyncOperationWrap wrap in asyncOpeCompletedList)
            {
                wrap.Call();
            }

            asyncOpeCompletedList.Clear();
        }

        #endregion Yield Request

        protected override void OnInit()
        {
            PlayerLoopManager.Instance.AddPlayerLoop(typeof(UnityEngine.PlayerLoop.EarlyUpdate), OnEarlyUpdate);

            earlyUpdateGroup = new UpdateGroup("EarlyUpdate");
            preUpdateGroup = new UpdateGroup("PreUpdate");
            updateGroup = new UpdateGroup("Update");
            fixedUpdateGroup = new UpdateGroup("FixedUpdate");
            lateUpdateGroup = new UpdateGroup("LateUpdate");

            asyncOpeWrapList = new List<AsyncOperationWrap>();
            asyncOpeCompletedList = new List<AsyncOperationWrap>();
        }

        protected override void OnDispose()
        {
            earlyUpdateGroup = null;
            preUpdateGroup = null;
            updateGroup = null;
            fixedUpdateGroup = null;
            lateUpdateGroup = null;

            asyncOpeWrapList = null;
            asyncOpeCompletedList = null;
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