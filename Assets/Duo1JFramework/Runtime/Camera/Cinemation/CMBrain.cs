using Cinemachine;
using Duo1JFramework.Asset;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.CinemachineBrain;

namespace Duo1JFramework.CameraAPI
{
    /// <summary>
    /// CinemachineBarin
    /// </summary>
    public class CMBrain : MonoSingleton<CMBrain>
    {
        public CinemachineBrain Brain { get; private set; }

        /// <summary>
        /// 当前默认相机
        /// </summary>
        public CMCamera Camera => CameraManager.Instance.Camera as CMCamera;

        /// <summary>
        /// 当前激活相机
        /// </summary>
        public CMCamera LiveCamera { get; private set; }

        /// <summary>
        /// 虚拟相机列表
        /// </summary>
        private HashSet<CMCamera> CameraSet;

        /// <summary>
        /// 忽略时间缩放
        /// </summary>
        public bool IgnoreTimeScale
        {
            get => Brain.m_IgnoreTimeScale;
            set => Brain.m_IgnoreTimeScale = value;
        }

        /// <summary>
        /// 更新类型
        /// </summary>
        public UpdateMethod UpdateType
        {
            get => Brain.m_UpdateMethod;
            set => Brain.m_UpdateMethod = value;
        }

        /// <summary>
        /// 融混更新类型
        /// </summary>
        public BrainUpdateMethod BlendUpdateType
        {
            get => Brain.m_BlendUpdateMethod;
            set => Brain.m_BlendUpdateMethod = value;
        }

        /// <summary>
        /// 设置激活相机
        /// </summary>
        public void SetLiveCamera(CMCamera _cmCamera)
        {
            if (LiveCamera != null)
            {
                LiveCamera.ResetTempPriority();
            }

            int maxPriority = int.MinValue;
            foreach (CMCamera cmCamera in CameraSet)
            {
                if (cmCamera.VirtualCamera.Priority > maxPriority)
                {
                    maxPriority = cmCamera.VirtualCamera.Priority;
                }
            }
            _cmCamera.SetTempPriority(maxPriority + 1);

            LiveCamera = _cmCamera;
        }

        /// <summary>
        /// 通过优先级设置激活相机
        /// </summary>
        /// <param name="resetPriority">重置所有虚拟相机优先级为默认</param>
        public void SetLiveCameraByPriority(bool resetPriority = false)
        {
            if (resetPriority)
            {
                foreach (CMCamera cmCamera in CameraSet)
                {
                    cmCamera.ResetTempPriority();
                }
            }

            int maxPriority = int.MinValue;
            CMCamera target = null;
            foreach (CMCamera cmCamera in CameraSet)
            {
                if (cmCamera.VirtualCamera.Priority > maxPriority)
                {
                    maxPriority = cmCamera.VirtualCamera.Priority;
                    target = cmCamera;
                }
                else if (cmCamera.VirtualCamera.Priority == maxPriority)
                {
                    if (target == Camera)
                    {
                        continue;
                    }
                    if (cmCamera == Camera)
                    {
                        maxPriority = cmCamera.VirtualCamera.Priority;
                        target = cmCamera;
                    }
                }
            }

            if (target != null)
            {
                SetLiveCamera(target);
            }
        }

        /// <summary>
        /// 设置激活相机为默认相机
        /// </summary>
        public void SetLiveCameraToDefault()
        {
            SetLiveCamera(Camera);
        }

        /// <summary>
        /// 设置融混方式
        /// </summary>
        public void SetBlend(CinemachineBlendDefinition.Style blendType, float time)
        {
            Brain.m_DefaultBlend = new CinemachineBlendDefinition(blendType, time);
        }

        /// <summary>
        /// 获取当前融混方式
        /// </summary>
        public CinemachineBlendDefinition GetBlend()
        {
            return Brain.m_DefaultBlend;
        }

        /// <summary>
        /// 自定义融混
        /// </summary>
        public CinemachineBlenderSettings CustomBlend
        {
            get => Brain.m_CustomBlends;
            set => Brain.m_CustomBlends = value;
        }

        /// <summary>
        /// 加载虚拟相机预制体
        /// </summary>
        public CinemachineVirtualCamera LoadVirtualCamera(string prefabPath)
        {
            GameObject cameraGo = AssetManager.Instance.LoadInsSync<GameObject>(prefabPath);
            if (cameraGo == null)
            {
                return null;
            }
            CinemachineVirtualCamera ret = cameraGo.GetComponent<CinemachineVirtualCamera>();
            if (ret == null)
            {
                Log.ErrorForce($"无法从{cameraGo.name}上获取到CinemachineVirtualCamera组件");
                cameraGo.DestroyImmediate();
                return null;
            }
            cameraGo.SetParent(Root.VirtualCameraRoot);
            return ret;
        }

        /// <summary>
        /// 通过资源路径创建相机
        /// </summary>
        public CMCamera CreateCamera(string prefabPath)
        {
            CMCamera camera = new CMCamera();
            camera.InitCamera(prefabPath);
            CreateCameraPostProcess(camera);
            return camera;
        }

        /// <summary>
        /// 通过虚拟相机创建相机
        /// </summary>
        public CMCamera CreateCamera(CinemachineVirtualCamera virtualCamera)
        {
            CMCamera camera = new CMCamera();
            camera.InitCamera(virtualCamera);
            CreateCameraPostProcess(camera);
            return camera;
        }

        /// <summary>
        /// 添加虚拟相机
        /// </summary>
        public void AddCMCamera(CMCamera cmCamera)
        {
            CameraSet.Add(cmCamera);
        }

        /// <summary>
        /// 移除虚拟相机
        /// </summary>
        public bool RemoveCMCamera(CMCamera cmCamera)
        {
            cmCamera.DestroyCamera();
            return CameraSet.Remove(cmCamera);
        }

        /// <summary>
        /// 创建虚拟相机后处理
        /// </summary>
        private void CreateCameraPostProcess(CMCamera cmCamera)
        {
            if (LiveCamera == null && cmCamera == Camera)
            {
                SetLiveCamera(cmCamera);
            }
        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
            CameraSet = new HashSet<CMCamera>();
            Camera mainCamera = CameraManager.Instance.GetOrCreateMainCamera();
            Brain = mainCamera.gameObject.GetOrAddComponent<CinemachineBrain>();
        }
    }
}
