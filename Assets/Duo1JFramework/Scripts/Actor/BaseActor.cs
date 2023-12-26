using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色基类
    /// </summary>
    public abstract class BaseActor
    {
        /// <summary>
        /// 管理器控制ID
        /// </summary>
        public long ID { get; private set; }
        /// <summary>
        /// 角色配置数据
        /// </summary>
        public ActorData Data { get; private set; }

        /// <summary>
        /// 角色物体
        /// </summary>
        public GameObject Go { get; private set; }
        /// <summary>
        /// 角色控制器
        /// </summary>
        public ActorController Controller { get; private set; }

        /// <summary>
        /// 是否已销毁
        /// </summary>
        public bool Disposed { get; private set; }

        /// <summary>
        /// 初始化Actor
        /// </summary>
        public void Init(long id, ActorData actorData)
        {
            ID = id;
            Data = actorData;
            OnInit();
        }

        /// <summary>
        /// 创建
        /// </summary>
        public void Create()
        {
            Disposed = false;
            BeforeCreate();
            //todo
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            OnDispose();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        protected virtual void OnInit()
        {
        }

        /// <summary>
        /// 创建前
        /// </summary>
        protected virtual void BeforeCreate()
        {
        }

        /// <summary>
        /// 创建完成
        /// </summary>
        protected virtual void OnCreated()
        {
        }

        /// <summary>
        /// 销毁
        /// </summary>
        protected abstract void OnDispose();
    }
}