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
        public GameObject AssetGo { get; private set; }

        /// <summary>
        /// 角色预制体Tf
        /// </summary>
        public Transform AssetTf => AssetGo == null ? null : AssetGo.transform;

        /// <summary>
        /// 角色模型Go
        /// </summary>
        public GameObject Model => Controller == null ? null : Controller.Model;

        /// <summary>
        /// 角色模型Tf
        /// </summary>
        public Transform ModelTf => Model == null ? null : Model.transform;

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

        /// <summary>
        /// 输入源
        /// </summary>
        public IActorInput InputSource { get; private set; } = new DefaultActorInput();

        /// <summary>
        /// 生命周期状态
        /// </summary>
        public EActorLifeState LifeState { get; private set; } = EActorLifeState.None;

        /// <summary>
        /// 设置输入源
        /// </summary>
        public BaseActor SetInputSource(IActorInput inputSource)
        {
            InputSource = inputSource ?? new DefaultActorInput();
            return this;
        }

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
                LifeState = EActorLifeState.Loading;
                Disposed = false;
                LoadAsset();
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
                IAssetHandle<GameObject> handle = Asset.LoadByTypeSync<GameObject>(Data.Path, Data.LoadType);
                LoadAssetPostprocess(handle);
            }
            else
            {
                Asset.LoadByType<GameObject>(Data.Path, (handle) =>
                {
                    LoadAssetPostprocess(handle);
                }, Data.LoadType);
            }
        }

        /// <summary>
        /// 加载Actor预制体资源后处理
        /// </summary>
        private void LoadAssetPostprocess(IAssetHandle<GameObject> handle)
        {
            Assert.NotNull(handle, $"{ToString()} 资源加载失败: `{Data.Path}`");

            if (Disposed || LifeState != EActorLifeState.Loading)
            {
                handle.Release();
                return;
            }

            AssetGo = handle.Instantiate();
            if (AssetGo == null)
            {
                handle.Release();
                return;
            }

            AssetGo.name = $"{Data.Name}-{ID} ({AssetGo.name})";
            AssetGo.SetParent(Root.ActorRoot);
            AssetGo.ResetSRT();

            Controller = AssetGo.GetAndAssertComponent<BaseActorController>($"{ToString()}资源预制体上未挂载ActorController组件");
            Controller.Logic = this;

            Param = Controller.Param;
            Point = Controller.Point;
            LifeState = EActorLifeState.Created;

            OnCreated();
        }

        /// <summary>
        /// 销毁资源
        /// </summary>
        protected void UnLoadAsset()
        {
            if (LifeState == EActorLifeState.Disposed)
            {
                return;
            }

            LifeState = EActorLifeState.Disposed;
            UnBindCamera();
            BeforeUnLoadAsset();

            if (AssetGo != null)
            {
                AssetGo.DestroyImmediate();
                AssetGo = null;
            }
            Controller = null;
            Param = null;
            Point = null;

            AfterUnLoadAsset();
        }

        #endregion Inner
    }
}
