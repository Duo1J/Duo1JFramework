using Duo1JFramework.Asset;
using Duo1JFramework.World;
using System;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色基类
    /// </summary>
    public abstract class BaseActor : BaseRegister
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
        /// 同步创建
        /// </summary>
        public bool Sync { get; set; } = false;

        /// <summary>
        /// 角色物体
        /// </summary>
        public GameObject Asset { get; private set; }

        /// <summary>
        /// 角色控制器
        /// </summary>
        public ActorController Controller { get; private set; }

        /// <summary>
        /// 初始化Actor
        /// </summary>
        public BaseActor Init(long id, ActorData actorData)
        {
            ID = id;
            Data = actorData;
            OnInit();
            return this;
        }

        /// <summary>
        /// 创建
        /// </summary>
        public BaseActor Create()
        {
            Assert.NotNull(Data, "Actor数据ActorData为空，无法创建");

            try
            {
                BeforeCreate();
                LoadAsset();
                Disposed = false;
                return this;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e);
                UnLoadAsset();
                return this;
            }
        }

        /// <summary>
        /// 加载Actor预制体资源
        /// </summary>
        protected void LoadAsset()
        {
            if (Sync)
            {
                GameObject asset = AssetManager.Instance.LoadSync<GameObject>(Data.Path);
                LoadAssetPostprocess(asset);
            }
            else
            {
                AssetManager.Instance.Load<GameObject>(Data.Path, (asset) =>
                {
                    LoadAssetPostprocess(asset);
                });
            }
        }

        /// <summary>
        /// 加载Actor预制体资源后处理
        /// </summary>
        /// <param name="asset"></param>
        private void LoadAssetPostprocess(GameObject asset)
        {
            Assert.NotNull(asset, $"Actor资源加载失败:{Data.Path}");

            Asset = asset;
            Asset.name = $"{Data.Name}-{ID} ({Asset.name})";
            Asset.SetParent(WorldManager.Instance.GetActorRoot());
            Asset.ResetSRT();

            Controller = Asset.GetAndAssertComponent<ActorController>("Actor资源预制体上未挂载ActorController组件");

            OnCreated();
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        protected void UnLoadAsset()
        {
            BeforeUnLoadAsset();

            if (Asset != null)
            {
                Asset.DestroyImmediate();
                Asset = null;
            }
            Controller = null;

            AfterUnLoadAsset();
        }

        /// <summary>
        /// 销毁
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            OnDispose();
            UnLoadAsset();
        }

        #region 子类override

        /// <summary>
        /// 子类初始化
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
        /// 创建完成后
        /// </summary>
        protected virtual void OnCreated()
        {
        }

        /// <summary>
        /// 卸载资源前
        /// </summary>
        protected virtual void BeforeUnLoadAsset()
        {
        }

        /// <summary>
        /// 卸载资源后
        /// </summary>
        protected virtual void AfterUnLoadAsset()
        {
        }

        /// <summary>
        /// 子类销毁
        /// </summary>
        protected override void OnDispose()
        {
        }

        #endregion 子类override
    }
}