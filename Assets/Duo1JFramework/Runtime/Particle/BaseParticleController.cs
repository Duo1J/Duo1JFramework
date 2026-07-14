using Duo1JFramework.Asset;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.ParticleAPI
{
    /// <summary>
    /// 基础粒子控制器
    /// </summary>
    public abstract class BaseParticleController : MonoRegister
    {
        /// <summary>
        /// 播放类型
        /// </summary>
        [SerializeField]
        protected EParticlePlayType particlePlayType = EParticlePlayType.OneShot;

        /// <summary>
        /// 播放类型
        /// </summary>
        public EParticlePlayType ParticlePlayType => particlePlayType;

        /// <summary>
        /// 播放数据
        /// </summary>
        protected ParticleData particleData;

        /// <summary>
        /// 播放数据
        /// </summary>
        public ParticleData ParticleData => particleData;

        /// <summary>
        /// 粒子实例
        /// </summary>
        protected GameObject particleGo;

        /// <summary>
        /// 粒子实例
        /// </summary>
        public GameObject ParticleGo => particleGo;

        /// <summary>
        /// 粒子系统列表
        /// </summary>
        protected readonly List<ParticleSystem> particleSystems = new List<ParticleSystem>();

        /// <summary>
        /// 是否正在播放
        /// </summary>
        public bool Playing
        {
            get
            {
                if (particleGo == null)
                {
                    return false;
                }
                for (int i = 0; i < particleSystems.Count; i++)
                {
                    if (particleSystems[i] != null && particleSystems[i].isPlaying)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 加载版本
        /// </summary>
        private int playVersion = 0;

        /// <summary>
        /// 是否已暂停
        /// </summary>
        private bool paused = false;

        /// <summary>
        /// 运行时速度
        /// </summary>
        private float runtimeSpeed = 1f;

        /// <summary>
        /// 跟随目标
        /// </summary>
        private Transform followTarget;

        /// <summary>
        /// OneShot自动回收计时
        /// </summary>
        private float oneShotTimer = 0f;

        /// <summary>
        /// OneShot最长存活
        /// </summary>
        private float oneShotLifeTime = 0f;

        /// <summary>
        /// 是否处于OneShot计时
        /// </summary>
        private bool oneShotPlaying = false;

        /// <summary>
        /// 持续播放
        /// </summary>
        public void PlayKeep(ParticleData data)
        {
            particlePlayType = EParticlePlayType.Keep;
            SetParticleDataAndLoad(data, Play);
        }

        /// <summary>
        /// 单次播放
        /// </summary>
        public void PlayOneShot(ParticleData data)
        {
            particlePlayType = EParticlePlayType.OneShot;
            SetParticleDataAndLoad(data, Play);
        }

        /// <summary>
        /// 在世界坐标单次播放
        /// </summary>
        public void PlayOneShotAt(ParticleData data, Vector3 position, Quaternion rotation)
        {
            transform.SetPositionAndRotation(position, rotation);
            followTarget = null;
            PlayOneShot(data);
        }

        /// <summary>
        /// 跟随目标持续播放
        /// </summary>
        public void PlayKeepAt(ParticleData data, Transform target)
        {
            followTarget = target;
            if (followTarget != null)
            {
                transform.SetPositionAndRotation(followTarget.position, followTarget.rotation);
            }
            PlayKeep(data);
        }

        /// <summary>
        /// 加载资源并准备播放
        /// </summary>
        protected void SetParticleDataAndLoad(ParticleData data, Action finCall = null)
        {
            if (data == null)
            {
                Log.ErrorForce($"{ToString()} ParticleData为空, 无法播放");
                Stop();
                return;
            }

            if (string.IsNullOrEmpty(data.ParticlePath))
            {
                Log.ErrorForce($"{ToString()} ParticlePath为空, 无法播放");
                Stop();
                return;
            }

            playVersion++;
            int currentVersion = playVersion;

            ClearParticleGo();
            particleData = data;
            paused = false;
            oneShotPlaying = false;

            if (data.Sync)
            {
                IAssetHandle<GameObject> handle = AssetManager.Instance.LoadByTypeSync<GameObject>(data.ParticlePath, data.LoadType);
                if (ParticlePrefabLoadedPostprocess(handle, currentVersion))
                {
                    finCall?.Invoke();
                }
            }
            else
            {
                AssetManager.Instance.LoadByType<GameObject>(data.ParticlePath, (handle) =>
                {
                    if (ParticlePrefabLoadedPostprocess(handle, currentVersion))
                    {
                        finCall?.Invoke();
                    }
                }, data.LoadType);
            }
        }

        /// <summary>
        /// 预制体加载完成后处理
        /// </summary>
        protected bool ParticlePrefabLoadedPostprocess(IAssetHandle<GameObject> handle, int loadVersion)
        {
            if (loadVersion != playVersion)
            {
                handle?.Release();
                return false;
            }

            if (handle == null || handle.Error())
            {
                Log.ErrorForce($"{particleData} 加载失败");
                handle?.Release();
                Stop();
                return false;
            }

            particleGo = handle.Instantiate();
            handle.Release();

            if (particleGo == null)
            {
                Log.ErrorForce($"{particleData} 实例化失败");
                Stop();
                return false;
            }

            Transform tf = particleGo.transform;
            tf.SetParent(transform, false);
            tf.localPosition = Vector3.zero;
            tf.localRotation = Quaternion.identity;

            particleSystems.Clear();
            particleGo.GetComponentsInChildren(true, particleSystems);
            ApplyParticleDataSetting();

            return true;
        }

        /// <summary>
        /// 应用播放参数
        /// </summary>
        protected void ApplyParticleDataSetting()
        {
            if (particleData == null || particleGo == null)
            {
                return;
            }

            particleGo.transform.localScale = Vector3.one * particleData.Scale;
            runtimeSpeed = particleData.Speed;
            RefreshSpeed();
        }

        /// <summary>
        /// 刷新播放速度
        /// </summary>
        public void RefreshSpeed()
        {
            if (particleData == null)
            {
                return;
            }

            float groupScale = 1f;
            if (ParticleManager.TryGetInstance(out ParticleManager particleManager))
            {
                groupScale = particleManager.GetFinalTimeScale(particleData.Category);
            }

            float speed = Mathf.Max(0f, runtimeSpeed * groupScale);
            for (int i = 0; i < particleSystems.Count; i++)
            {
                ParticleSystem ps = particleSystems[i];
                if (ps == null)
                {
                    continue;
                }
                ParticleSystem.MainModule main = ps.main;
                main.simulationSpeed = speed;
            }
        }

        /// <summary>
        /// 刷新显隐
        /// </summary>
        public void RefreshHidden()
        {
            if (particleData == null || particleGo == null)
            {
                return;
            }

            bool hidden = false;
            if (ParticleManager.TryGetInstance(out ParticleManager particleManager))
            {
                hidden = particleManager.GetFinalHidden(particleData.Category);
            }
            particleGo.SetActive(!hidden);
        }

        /// <summary>
        /// 播放
        /// </summary>
        public void Play()
        {
            if (particleGo == null)
            {
                Log.ErrorForce($"{ToString()} ParticleGo为空, 无法播放");
                Stop();
                return;
            }

            particleGo.SetActive(true);
            for (int i = 0; i < particleSystems.Count; i++)
            {
                if (particleSystems[i] != null)
                {
                    particleSystems[i].Play(true);
                }
            }
            paused = false;

            if (particlePlayType == EParticlePlayType.OneShot)
            {
                oneShotPlaying = true;
                oneShotTimer = 0f;
                oneShotLifeTime = CalcOneShotLifeTime();
            }

            RefreshHidden();

            if (ParticleManager.TryGetInstance(out ParticleManager particleManager)
                && particleManager.ShouldPause(particleData.Category) && !particleData.IgnorePause)
            {
                Pause();
            }

            OnPlay();
        }

        protected virtual void OnPlay()
        {
            if (ParticleManager.TryGetInstance(out ParticleManager particleManager) && this is ParticleMgrController controller)
            {
                particleManager.RegisterActiveController(controller);
            }
        }

        /// <summary>
        /// 暂停
        /// </summary>
        public void Pause()
        {
            if (paused)
            {
                return;
            }

            for (int i = 0; i < particleSystems.Count; i++)
            {
                if (particleSystems[i] != null)
                {
                    particleSystems[i].Pause(true);
                }
            }
            paused = true;

            OnPause();
        }

        protected virtual void OnPause()
        {
        }

        /// <summary>
        /// 继续播放
        /// </summary>
        public void Resume()
        {
            if (!paused)
            {
                return;
            }

            for (int i = 0; i < particleSystems.Count; i++)
            {
                if (particleSystems[i] != null)
                {
                    particleSystems[i].Play(true);
                }
            }
            paused = false;

            OnResume();
        }

        protected virtual void OnResume()
        {
        }

        /// <summary>
        /// 按分组暂停
        /// </summary>
        public void PauseByGroup()
        {
            if (particleData != null && particleData.IgnorePause)
            {
                return;
            }
            Pause();
        }

        /// <summary>
        /// 停止
        /// </summary>
        public void Stop()
        {
            Stop(false);
        }

        /// <summary>
        /// 停止
        /// </summary>
        protected void Stop(bool invokeFinishCallback)
        {
            ParticleData stoppedData = particleData;

            playVersion++;
            ClearParticleGo();
            particleData = null;
            followTarget = null;
            paused = false;
            oneShotPlaying = false;

            OnStop();

            if (invokeFinishCallback)
            {
                stoppedData?.FinishCallback?.Invoke(stoppedData);
            }
        }

        protected virtual void OnStop()
        {
            if (ParticleManager.TryGetInstance(out ParticleManager particleManager) && this is ParticleMgrController controller)
            {
                particleManager.UnRegisterActiveController(controller);
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        public void Clear()
        {
            playVersion++;
            ClearParticleGo();
            particleData = null;
            followTarget = null;
            particlePlayType = EParticlePlayType.OneShot;
            paused = false;
            oneShotPlaying = false;
            runtimeSpeed = 1f;

            OnClear();
        }

        protected virtual void OnClear()
        {
            if (ParticleManager.TryGetInstance(out ParticleManager particleManager) && this is ParticleMgrController controller)
            {
                particleManager.UnRegisterActiveController(controller);
            }
        }

        protected void OnUpdate()
        {
            if (followTarget != null && particleGo != null)
            {
                transform.SetPositionAndRotation(followTarget.position, followTarget.rotation);
            }

            if (oneShotPlaying && !paused)
            {
                oneShotTimer += Time.deltaTime;
                if (oneShotTimer >= oneShotLifeTime && !AnyAliveParticles())
                {
                    Stop(true);
                    return;
                }
            }

            OnSubUpdate();
        }

        protected virtual void OnSubUpdate()
        {
        }

        /// <summary>
        /// 是否还存在存活粒子
        /// </summary>
        private bool AnyAliveParticles()
        {
            for (int i = 0; i < particleSystems.Count; i++)
            {
                ParticleSystem ps = particleSystems[i];
                if (ps != null && ps.IsAlive(true))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 估算OneShot最长存活时间
        /// </summary>
        private float CalcOneShotLifeTime()
        {
            if (particleData != null && particleData.AutoRecycleTime > 0f)
            {
                return particleData.AutoRecycleTime;
            }

            float maxLife = 0f;
            for (int i = 0; i < particleSystems.Count; i++)
            {
                ParticleSystem ps = particleSystems[i];
                if (ps == null)
                {
                    continue;
                }
                ParticleSystem.MainModule main = ps.main;
                float total = main.duration + main.startLifetime.constantMax;
                if (total > maxLife)
                {
                    maxLife = total;
                }
            }
            return maxLife;
        }

        /// <summary>
        /// 清理粒子实例
        /// </summary>
        private void ClearParticleGo()
        {
            particleSystems.Clear();
            if (particleGo != null)
            {
                particleGo.DestroySmart();
                particleGo = null;
            }
        }

        private void Awake()
        {
            Reg.RegisterUpdate(OnUpdate);
            Clear();
            OnAwake();
        }

        protected virtual void OnAwake()
        {
        }
    }
}
