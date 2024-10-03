using Duo1JFramework.Asset;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界场景管理器
    /// </summary>
    public class WorldManager : MonoSingleton<WorldManager>
    {
        /// <summary>
        /// 世界场景控制器字典
        /// </summary>
        private Dictionary<string, BaseWorldController> worldDict;

        /// <summary>
        /// 加载世界场景
        /// </summary>
        public void LoadWorld(WorldData worldData, Action<BaseWorldController> callback)
        {
            Assert.NotNull(worldData, "参数worldData不可为空");
            Assert.NotNull(callback, "LoadWorld回调不可为空");

            if (worldDict.ContainsKey(worldData.Name))
            {
                callback(worldDict[worldData.Name]);
                return;
            }

            LoadWorldAsset(worldData, callback);
        }

        /// <summary>
        /// 销毁世界场景
        /// </summary>
        public void DestroyWorld(string worldName)
        {
            if (!worldDict.ContainsKey(worldName))
            {
                return;
            }

            BaseWorldController controller = worldDict[worldName];
            worldDict.Remove(worldName);
            controller.Destroy();
        }

        /// <summary>
        /// 获取世界场景控制器
        /// </summary>
        public BaseWorldController GetWorld(string worldName)
        {
            if (!worldDict.ContainsKey(worldName))
            {
                Log.ErrorForce($"未找到名为`{worldName}`的世界");
                return null;
            }

            return worldDict[worldName];
        }

        #region Inner

        /// <summary>
        /// 加载世界资源
        /// </summary>
        private void LoadWorldAsset(WorldData worldData, Action<BaseWorldController> callback)
        {
            if (worldData.Sync)
            {
                GameObject go = AssetManager.Instance.LoadInsByTypeSync<GameObject>(worldData.Path, worldData.LoadType);
                LoadWorldAssetPostProcess(go, worldData, callback);
            }
            else
            {
                AssetManager.Instance.LoadInsByType<GameObject>(worldData.Path, (go) =>
                {
                    LoadWorldAssetPostProcess(go, worldData, callback);
                }, worldData.LoadType);
            }
        }

        /// <summary>
        /// 加载世界资源后处理
        /// </summary>
        private void LoadWorldAssetPostProcess(GameObject go, WorldData worldData, Action<BaseWorldController> callback)
        {
            if (go == null)
            {
                Log.ErrorForce($"加载世界`{worldData.Path}`失败");
                callback(null);
                return;
            }

            go.SetParent(Root.WorldRoot);

            BaseWorldController controller = go.GetComponent<BaseWorldController>();
            if (controller == null)
            {
                Log.Warn($"世界`{worldData.Path}`未挂载`BaseWorldController`的派生组件, 默认挂载`DefaultWorldController`");
                controller = go.AddComponent<DefaultWorldController>();
            }

            controller.Init(worldData);
            worldDict.Add(worldData.Name, controller);

            callback(controller);
        }

        protected override void OnDispose()
        {
            worldDict.Clear();
            worldDict = null;
        }

        protected override void OnInit()
        {
            worldDict = new Dictionary<string, BaseWorldController>();
        }

        #endregion Inner
    }
}
