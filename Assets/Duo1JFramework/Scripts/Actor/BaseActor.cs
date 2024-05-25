using Duo1JFramework.Asset;
using Duo1JFramework.CameraAPI;
using Duo1JFramework.GamerInput;
using Duo1JFramework.UI;
using Duo1JFramework.World;
using System;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色逻辑基类
    /// </summary>
    [Serializable]
    public abstract class BaseActor : BaseRegister,
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
        /// 同步创建
        /// </summary>
        public bool Sync { get; set; } = false;

        /// <summary>
        /// 角色预制体Go
        /// </summary>
        public GameObject Asset { get; private set; }

        /// <summary>
        /// 角色预制体Tf
        /// </summary>
        public Transform AssetTf => Asset.transform;

        public Vector3 Pos => AssetTf.position;
        public Vector3 LocPos => AssetTf.localPosition;
        public Quaternion Rot => AssetTf.rotation;
        public Quaternion LocRot => AssetTf.localRotation;
        public Vector3 Angle => AssetTf.eulerAngles;
        public Vector3 LocAngle => AssetTf.localEulerAngles;
        public Vector3 Scale => AssetTf.lossyScale;
        public Vector3 LocScale => AssetTf.localScale;

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
        public ActorController Controller { get; private set; }

        /// <summary>
        /// 角色参数
        /// </summary>
        public ActorParam Param { get; private set; }

        /// <summary>
        /// 是否绑定了相机
        /// </summary>
        public bool CameraBinded { get; private set; }

        public virtual Transform CameraFollowPoint => GetCameraPoint();

        public virtual Transform CameraLookAtPoint => GetCameraPoint();

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
                GameObject asset = AssetManager.Instance.LoadInsSync<GameObject>(Data.Path);
                LoadAssetPostprocess(asset);
            }
            else
            {
                AssetManager.Instance.LoadIns<GameObject>(Data.Path, (asset) =>
                {
                    LoadAssetPostprocess(asset);
                });
            }
        }

        /// <summary>
        /// 加载Actor预制体资源后处理
        /// </summary>
        private void LoadAssetPostprocess(GameObject asset)
        {
            Assert.NotNull(asset, $"Actor资源加载失败:{Data.Path}");

            Asset = asset;
            Asset.name = $"{Data.Name}-{ID} ({Asset.name})";
            Asset.SetParent(WorldManager.Instance.ActorRoot);
            Asset.ResetSRT();

            Controller = Asset.GetAndAssertComponent<ActorController>("Actor资源预制体上未挂载ActorController组件");
            Controller.Actor = this;

            Param = Controller.GetActorParam();

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

        public override string ToString()
        {
            return $"<Actor-{ID}-{Data.Name}-{Data.LogicType}><Con-{Controller}>";
        }

        /// <summary>
        /// 更新相机
        /// </summary>
        protected virtual void UpdateCamera()
        {
            float mx = InputManager.GetAxisMX();
            float my = InputManager.GetAxisMY();
            Controller.RotateCameraPoint(mx, my);
            Controller.UpdateCameraPointPos();
        }

        /// <summary>
        /// 获取相机绑定点
        /// </summary>
        public Transform GetCameraPoint()
        {
            if (Controller == null)
            {
                return null;
            }
            return Controller.GetActorPoint().CameraPoint;
        }

        /// <summary>
        /// 绑定相机
        /// </summary>
        public virtual void BindCamera()
        {
            CameraManager.Instance.LookAt = this;
            CameraManager.Instance.Follow = this;
            CameraBinded = true;
        }

        /// <summary>
        /// 解绑相机
        /// </summary>
        public virtual void UnBindCamera()
        {
            CameraBinded = false;
            if (CameraManager.Instance.LookAt == this)
            {
                CameraManager.Instance.LookAt = null;
            }
            if (CameraManager.Instance.Follow == this)
            {
                CameraManager.Instance.Follow = null;
            }
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

        #region ActorController接口

        public GameObject GetRootGo() => Controller.Root;

        public Animator GetAnimator() => Controller.GetAnimator();

        #endregion ActorController接口
    }
}