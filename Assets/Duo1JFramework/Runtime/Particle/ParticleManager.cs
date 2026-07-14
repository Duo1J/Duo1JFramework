using Duo1JFramework.ObjectPool;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.ParticleAPI
{
    /// <summary>
    /// 粒子管理器
    /// </summary>
    public class ParticleManager : MonoSingleton<ParticleManager>
    {
        /// <summary>
        /// ParticleController对象池
        /// </summary>
        private GameObjectPool pool;

        /// <summary>
        /// 活跃控制器列表
        /// </summary>
        private readonly List<ParticleMgrController> activeControllerList = new List<ParticleMgrController>();

        /// <summary>
        /// 活跃控制器快照缓存
        /// </summary>
        private readonly List<ParticleMgrController> activeControllerSnapshotCache = new List<ParticleMgrController>();

        /// <summary>
        /// 粒子分组字典
        /// </summary>
        private readonly Dictionary<EParticleCategory, ParticleGroup> particleGroupDict = new Dictionary<EParticleCategory, ParticleGroup>();

        /// <summary>
        /// 路径最后播放时间
        /// </summary>
        private readonly Dictionary<string, float> particleLastPlayTimeDict = new Dictionary<string, float>();

        /// <summary>
        /// 持续播放
        /// </summary>
        public ParticleMgrController PlayKeep(ParticleData particleData)
        {
            if (!CanPlay(particleData))
            {
                return null;
            }

            ParticleMgrController controller = PopCon();
            controller.PlayKeep(particleData);
            MarkPlayed(particleData);

            return controller;
        }

        /// <summary>
        /// 跟随目标持续播放
        /// </summary>
        public ParticleMgrController PlayKeepAt(ParticleData particleData, Transform target)
        {
            if (!CanPlay(particleData))
            {
                return null;
            }

            ParticleMgrController controller = PopCon();
            controller.PlayKeepAt(particleData, target);
            MarkPlayed(particleData);

            return controller;
        }

        /// <summary>
        /// 单次播放
        /// </summary>
        public ParticleMgrController PlayOneShot(ParticleData particleData)
        {
            if (!CanPlay(particleData))
            {
                return null;
            }

            ParticleMgrController controller = PopCon();
            controller.PlayOneShot(particleData);
            MarkPlayed(particleData);

            return controller;
        }

        /// <summary>
        /// 在世界坐标单次播放
        /// </summary>
        public ParticleMgrController PlayOneShotAt(ParticleData particleData, Vector3 position, Quaternion rotation)
        {
            if (!CanPlay(particleData))
            {
                return null;
            }

            ParticleMgrController controller = PopCon();
            controller.PlayOneShotAt(particleData, position, rotation);
            MarkPlayed(particleData);

            return controller;
        }

        /// <summary>
        /// 在世界坐标单次播放
        /// </summary>
        public ParticleMgrController PlayOneShotAt(ParticleData particleData, Vector3 position)
        {
            return PlayOneShotAt(particleData, position, Quaternion.identity);
        }

        /// <summary>
        /// 停止所有持续播放
        /// </summary>
        public void StopAllKeep()
        {
            StopByPlayType(EParticlePlayType.Keep);
        }

        /// <summary>
        /// 停止所有单次播放
        /// </summary>
        public void StopAllOneShot()
        {
            StopByPlayType(EParticlePlayType.OneShot);
        }

        /// <summary>
        /// 注册活跃控制器
        /// </summary>
        public void RegisterActiveController(ParticleMgrController controller)
        {
            if (controller == null || activeControllerList.Contains(controller))
            {
                return;
            }
            activeControllerList.Add(controller);
        }

        /// <summary>
        /// 取消注册活跃控制器
        /// </summary>
        public void UnRegisterActiveController(ParticleMgrController controller)
        {
            if (controller == null)
            {
                return;
            }
            activeControllerList.Remove(controller);
        }

        /// <summary>
        /// 设置分类时间缩放
        /// </summary>
        public void SetTimeScale(EParticleCategory category, float timeScale)
        {
            GetParticleGroup(category).SetTimeScale(timeScale);
            RefreshSpeed(category);
        }

        /// <summary>
        /// 获取分类时间缩放
        /// </summary>
        public float GetTimeScale(EParticleCategory category)
        {
            return GetParticleGroup(category).TimeScale;
        }

        /// <summary>
        /// 获取最终时间缩放
        /// </summary>
        public float GetFinalTimeScale(EParticleCategory category)
        {
            return GetParticleGroup(category).TimeScale;
        }

        /// <summary>
        /// 设置分类隐藏
        /// </summary>
        public void SetHidden(EParticleCategory category, bool hidden)
        {
            GetParticleGroup(category).SetHidden(hidden);
            RefreshHidden(category);
        }

        /// <summary>
        /// 获取分类隐藏状态
        /// </summary>
        public bool GetHidden(EParticleCategory category)
        {
            return GetParticleGroup(category).Hidden;
        }

        /// <summary>
        /// 获取最终隐藏状态
        /// </summary>
        public bool GetFinalHidden(EParticleCategory category)
        {
            return GetParticleGroup(category).Hidden;
        }

        /// <summary>
        /// 设置分类暂停
        /// </summary>
        public void SetPause(EParticleCategory category, bool pause)
        {
            GetParticleGroup(category).SetPause(pause);

            foreach (ParticleMgrController controller in GetActiveControllerSnapshot())
            {
                if (controller.ParticleData == null)
                {
                    continue;
                }
                if (controller.ParticleData.Category != category)
                {
                    continue;
                }

                if (pause)
                {
                    controller.PauseByGroup();
                }
                else if (!ShouldPause(controller.ParticleData.Category))
                {
                    controller.Resume();
                }
            }
        }

        /// <summary>
        /// 获取分类暂停状态
        /// </summary>
        public bool GetPause(EParticleCategory category)
        {
            return GetParticleGroup(category).Paused;
        }

        /// <summary>
        /// 是否需要暂停指定分类
        /// </summary>
        public bool ShouldPause(EParticleCategory category)
        {
            return GetParticleGroup(category).Paused;
        }

        /// <summary>
        /// 停止指定分类
        /// </summary>
        public void StopByCategory(EParticleCategory category)
        {
            foreach (ParticleMgrController controller in GetActiveControllerSnapshot())
            {
                if (controller.ParticleData == null)
                {
                    continue;
                }
                if (controller.ParticleData.Category == category)
                {
                    controller.Stop();
                }
            }
        }

        /// <summary>
        /// 刷新分类速度
        /// </summary>
        private void RefreshSpeed(EParticleCategory category)
        {
            foreach (ParticleMgrController controller in GetActiveControllerSnapshot())
            {
                if (controller.ParticleData == null)
                {
                    continue;
                }
                if (controller.ParticleData.Category == category)
                {
                    controller.RefreshSpeed();
                }
            }
        }

        /// <summary>
        /// 刷新分类显隐
        /// </summary>
        private void RefreshHidden(EParticleCategory category)
        {
            foreach (ParticleMgrController controller in GetActiveControllerSnapshot())
            {
                if (controller.ParticleData == null)
                {
                    continue;
                }
                if (controller.ParticleData.Category == category)
                {
                    controller.RefreshHidden();
                }
            }
        }

        /// <summary>
        /// 停止指定播放类型
        /// </summary>
        private void StopByPlayType(EParticlePlayType playType)
        {
            foreach (ParticleMgrController controller in GetActiveControllerSnapshot())
            {
                if (controller.ParticlePlayType == playType)
                {
                    controller.Stop();
                }
            }
        }

        /// <summary>
        /// 是否可以播放
        /// </summary>
        private bool CanPlay(ParticleData particleData)
        {
            if (particleData == null)
            {
                Log.ErrorForce("ParticleData为空, 无法播放");
                return false;
            }

            if (string.IsNullOrEmpty(particleData.ParticlePath))
            {
                Log.ErrorForce("ParticlePath为空, 无法播放");
                return false;
            }

            if (particleData.Cooldown > 0f && particleLastPlayTimeDict.TryGetValue(particleData.ParticlePath, out float lastPlayTime))
            {
                if (Time.time - lastPlayTime < particleData.Cooldown)
                {
                    return false;
                }
            }

            if (particleData.MaxSameParticleCount > 0 && CountSameParticle(particleData.ParticlePath) >= particleData.MaxSameParticleCount)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 标记已播放
        /// </summary>
        private void MarkPlayed(ParticleData particleData)
        {
            if (particleData == null || string.IsNullOrEmpty(particleData.ParticlePath))
            {
                return;
            }
            particleLastPlayTimeDict[particleData.ParticlePath] = Time.time;
        }

        /// <summary>
        /// 统计同路径粒子数量
        /// </summary>
        private int CountSameParticle(string particlePath)
        {
            int count = 0;
            foreach (ParticleMgrController controller in activeControllerList)
            {
                if (controller.ParticleData != null && controller.ParticleData.ParticlePath == particlePath)
                {
                    count++;
                }
            }
            return count;
        }

        /// <summary>
        /// 获取活跃控制器快照
        /// </summary>
        private List<ParticleMgrController> GetActiveControllerSnapshot()
        {
            activeControllerSnapshotCache.Clear();
            activeControllerSnapshotCache.AddRange(activeControllerList);
            return activeControllerSnapshotCache;
        }

        /// <summary>
        /// 获取分组
        /// </summary>
        private ParticleGroup GetParticleGroup(EParticleCategory category)
        {
            if (!particleGroupDict.TryGetValue(category, out ParticleGroup group))
            {
                group = new ParticleGroup(category);
                particleGroupDict.Add(category, group);
            }
            return group;
        }

        protected override void OnDispose()
        {
            foreach (ParticleMgrController controller in GetActiveControllerSnapshot())
            {
                controller.Stop();
            }

            activeControllerList.Clear();
            activeControllerSnapshotCache.Clear();
            particleLastPlayTimeDict.Clear();
            particleGroupDict.Clear();
        }

        public ParticleMgrController PopCon()
        {
            return pool.Pop().GetComponent<ParticleMgrController>();
        }

        public void PushCon(ParticleMgrController controller)
        {
            if (controller == null)
            {
                return;
            }

            UnRegisterActiveController(controller);
            pool.Push(controller.gameObject);
        }

        /// <summary>
        /// Particle物体出池初始化
        /// </summary>
        private GameObject OnPopParticleGo(GameObject go)
        {
            ParticleMgrController controller = go.GetComponent<ParticleMgrController>();
            if (controller == null)
            {
                controller = go.AddComponent<ParticleMgrController>();
            }
            else
            {
                controller.Clear();
            }

            go.SetActive(true);
            return go;
        }

        protected override void OnInit()
        {
            foreach (EParticleCategory category in Enum.GetValues(typeof(EParticleCategory)))
            {
                GetParticleGroup(category);
            }

            GameObject particleTemplate = new GameObject("ParticleTemplate");
            particleTemplate.AddComponent<ParticleMgrController>();
            pool = new GameObjectPool(particleTemplate, OnPopParticleGo, transform);
        }
    }
}
