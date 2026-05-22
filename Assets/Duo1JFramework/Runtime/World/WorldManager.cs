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
        /// 世界加载开始事件
        /// </summary>
        public event Action<WorldData> OnWorldLoadStart;

        /// <summary>
        /// 世界加载成功事件
        /// </summary>
        public event Action<BaseWorldController> OnWorldLoaded;

        /// <summary>
        /// 世界加载失败事件
        /// </summary>
        public event Action<WorldLoadResult> OnWorldLoadFailed;

        /// <summary>
        /// 世界销毁前事件
        /// </summary>
        public event Action<BaseWorldController> OnWorldBeforeDestroy;

        /// <summary>
        /// 世界销毁完成事件
        /// </summary>
        public event Action<string> OnWorldDestroyed;

        /// <summary>
        /// 世界场景控制器字典
        /// </summary>
        private Dictionary<string, BaseWorldController> worldDict;

        /// <summary>
        /// 加载中回调字典
        /// </summary>
        private Dictionary<string, List<Action<WorldLoadResult>>> loadingCallbackDict;

        /// <summary>
        /// 世界数量
        /// </summary>
        public int WorldCount => worldDict == null ? 0 : worldDict.Count;

        /// <summary>
        /// 加载中世界数量
        /// </summary>
        public int LoadingWorldCount => loadingCallbackDict == null ? 0 : loadingCallbackDict.Count;

        /// <summary>
        /// 加载世界场景
        /// </summary>
        public void LoadWorld(WorldData worldData, Action<WorldLoadResult> callback)
        {
            Assert.NotNullArg(worldData, "worldData");
            Assert.NotNullArg(callback, "callback");

            if (worldDict.ContainsKey(worldData.Name))
            {
                callback(WorldLoadResult.CreateSuccess(worldData, worldDict[worldData.Name]));
                return;
            }

            if (loadingCallbackDict.TryGetValue(worldData.Name, out List<Action<WorldLoadResult>> callbacks))
            {
                callbacks.Add(callback);
                return;
            }

            loadingCallbackDict.Add(worldData.Name, new List<Action<WorldLoadResult>>() { callback });
            OnWorldLoadStart?.Invoke(worldData);
            LoadWorldAsset(worldData);
        }

        /// <summary>
        /// 重载世界场景
        /// </summary>
        public void ReloadWorld(WorldData worldData, Action<WorldLoadResult> callback = null)
        {
            Assert.NotNullArg(worldData, "worldData");
            DestroyWorld(worldData.Name);
            LoadWorld(worldData, callback ?? (_result) => { });
        }

        /// <summary>
        /// 预加载世界场景
        /// </summary>
        public void PreloadWorld(WorldData worldData, Action<WorldLoadResult> callback = null)
        {
            LoadWorld(worldData, callback ?? (_result) => { });
        }

        /// <summary>
        /// 激活世界场景
        /// </summary>
        public bool ActivateWorld(string worldName)
        {
            if (!TryGetWorld(worldName, out BaseWorldController controller))
            {
                return false;
            }

            controller.gameObject.SetActive(true);
            return true;
        }

        /// <summary>
        /// 反激活世界场景
        /// </summary>
        public bool DeactivateWorld(string worldName)
        {
            if (!TryGetWorld(worldName, out BaseWorldController controller))
            {
                return false;
            }

            controller.gameObject.SetActive(false);
            return true;
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

            OnWorldBeforeDestroy?.Invoke(controller);
            controller.Destroy();
            OnWorldDestroyed?.Invoke(worldName);
        }

        /// <summary>
        /// 卸载所有世界场景
        /// </summary>
        public void UnloadAllWorlds()
        {
            List<string> worldNameList = new List<string>(worldDict.Keys);
            foreach (string worldName in worldNameList)
            {
                DestroyWorld(worldName);
            }
        }

        /// <summary>
        /// 获取世界场景控制器
        /// </summary>
        public BaseWorldController GetWorld(string worldName)
        {
            if (!TryGetWorld(worldName, out BaseWorldController controller))
            {
                Log.ErrorForce($"未找到名为`{worldName}`的世界");
                return null;
            }

            return controller;
        }

        /// <summary>
        /// 是否存在世界场景
        /// </summary>
        public bool HasWorld(string worldName)
        {
            return worldDict.ContainsKey(worldName);
        }

        /// <summary>
        /// 是否正在加载世界场景
        /// </summary>
        public bool IsWorldLoading(string worldName)
        {
            return loadingCallbackDict.ContainsKey(worldName);
        }

        /// <summary>
        /// 尝试获取世界场景控制器
        /// </summary>
        public bool TryGetWorld(string worldName, out BaseWorldController controller)
        {
            return worldDict.TryGetValue(worldName, out controller);
        }

        #region Inner

        /// <summary>
        /// 加载世界资源
        /// </summary>
        private void LoadWorldAsset(WorldData worldData)
        {
            if (worldData.Sync)
            {
                IAssetHandle<GameObject> handle = AssetManager.Instance.LoadByTypeSync<GameObject>(worldData.Path, worldData.LoadType);
                LoadWorldAssetPostProcess(handle, worldData);
            }
            else
            {
                AssetManager.Instance.LoadByType<GameObject>(worldData.Path, (handle) =>
                {
                    LoadWorldAssetPostProcess(handle, worldData);
                }, worldData.LoadType);
            }
        }

        /// <summary>
        /// 加载世界资源后处理
        /// </summary>
        private void LoadWorldAssetPostProcess(IAssetHandle<GameObject> handle, WorldData worldData)
        {
            if (!loadingCallbackDict.ContainsKey(worldData.Name))
            {
                handle?.Release();
                return;
            }

            if (handle == null || handle.Error())
            {
                CompleteLoad(WorldLoadResult.CreateFail(worldData, $"加载世界`{worldData.Path}`失败"));
                handle?.Release();
                return;
            }

            GameObject go = handle.Instantiate();
            handle.Release();
            handle = null;

            if (go == null)
            {
                CompleteLoad(WorldLoadResult.CreateFail(worldData, $"实例化世界`{worldData.Path}`失败"));
                return;
            }

            go.name = string.IsNullOrEmpty(worldData.InstanceName) ? worldData.Name : worldData.InstanceName;
            go.SetParent(Root.WorldRoot);

            BaseWorldController controller = go.GetComponent<BaseWorldController>();
            if (controller == null)
            {
                Log.Warn($"世界`{worldData.Path}`未挂载`BaseWorldController`的派生组件, 默认挂载`DefaultWorldController`");
                controller = go.AddComponent<DefaultWorldController>();
            }

            controller.Init(worldData);
            worldDict.Add(worldData.Name, controller);

            CompleteLoad(WorldLoadResult.CreateSuccess(worldData, controller));
        }

        private void CompleteLoad(WorldLoadResult result)
        {
            List<Action<WorldLoadResult>> callbacks = loadingCallbackDict[result.WorldData.Name];
            loadingCallbackDict.Remove(result.WorldData.Name);

            if (result.Success)
            {
                OnWorldLoaded?.Invoke(result.Controller);
            }
            else
            {
                Log.ErrorForce(result.Error);
                OnWorldLoadFailed?.Invoke(result);
            }

            foreach (Action<WorldLoadResult> callback in callbacks)
            {
                callback(result);
            }
        }

        protected override void OnDispose()
        {
            UnloadAllWorlds();
            loadingCallbackDict.Clear();
            loadingCallbackDict = null;
            worldDict = null;
        }

        protected override void OnInit()
        {
            worldDict = new Dictionary<string, BaseWorldController>();
            loadingCallbackDict = new Dictionary<string, List<Action<WorldLoadResult>>>();
        }

#if UNITY_EDITOR
        public void DrawEditorInfo()
        {
            GUILayout.Label($"世界数量: {WorldCount}");
            GUILayout.Label($"加载中世界数量: {LoadingWorldCount}");

            GUILayout.Space(5);

            GUILayout.Label("已加载世界列表");
            foreach (KeyValuePair<string, BaseWorldController> kv in worldDict)
            {
                BaseWorldController controller = kv.Value;
                ED.Vertical(() =>
                {
                    GUILayout.Label($"名称: {kv.Key}");
                    GUILayout.Label($"路径: {controller.WorldData.Path}");
                    GUILayout.Label($"激活: {controller.gameObject.activeSelf}");
                    GUILayout.Label($"暂停: {controller.Paused}");
                    GUILayout.Label($"控制器: {controller.GetType().Name}");
                }, "box");
            }

            GUILayout.Space(5);

            GUILayout.Label("加载中世界列表");
            foreach (string worldName in loadingCallbackDict.Keys)
            {
                GUILayout.Label(worldName);
            }
        }
#endif

        #endregion Inner
    }
}
