using Duo1JFramework.Asset;
using Duo1JFramework.CameraAPI;
using System;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色逻辑基类
    /// </summary>
    [Serializable]
    public abstract partial class BaseActor : BaseRegister,
        ICameraFollow,
        ICameraLookAt
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
        /// 角色预制体Go
        /// </summary>
        public GameObject Asset { get; private set; }

        /// <summary>
        /// 角色预制体Tf
        /// </summary>
        public Transform AssetTf => Asset.transform;

        /// <summary>
        /// 角色模型Go
        /// </summary>
        public GameObject Model => Controller.Model;

        /// <summary>
        /// 角色模型Tf
        /// </summary>
        public Transform ModelTf => Model.transform;

        /// <summary>
        /// 角色控制器
        /// </summary>
        public BaseActorController Controller { get; private set; }

        /// <summary>
        /// 角色参数
        /// </summary>
        public ActorParam Param { get; private set; }

        /// <summary>
        /// 角色挂点
        /// </summary>
        public ActorPoint Point { get; private set; }

        public override string ToString()
        {
            return $"<Actor-{ID}-{(Data == null ? "NullName" : Data.Name)}-{(Data == null ? "NullLogicType" : Data.LogicType.ToString())}><Con-{(Controller == null ? "NullController" : Controller.ToString())}>";
        }

        #region Inner

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
            Assert.NotNull(Data, $"{ToString()}数据ActorData为空，无法创建");

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
            if (Data.Sync)
            {
                GameObject asset = AssetManager.Instance.LoadInsByTypeSync<GameObject>(Data.Path, Data.LoadType);
                LoadAssetPostprocess(asset);
            }
            else
            {
                AssetManager.Instance.LoadInsByType<GameObject>(Data.Path, (asset) =>
                {
                    LoadAssetPostprocess(asset);
                }, Data.LoadType);
            }
        }

        /// <summary>
        /// 加载Actor预制体资源后处理
        /// </summary>
        private void LoadAssetPostprocess(GameObject asset)
        {
            Assert.NotNull(asset, $"{ToString()} 资源加载失败: `{Data.Path}`");

            Asset = asset;
            Asset.name = $"{Data.Name}-{ID} ({Asset.name})";
            Asset.SetParent(Root.ActorRoot);
            Asset.ResetSRT();

            Controller = Asset.GetAndAssertComponent<BaseActorController>($"{ToString()}资源预制体上未挂载ActorController组件");
            Controller.Logic = this;

            Param = Controller.Param;
            Point = Controller.Point;

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

        #endregion Inner
    }
}
