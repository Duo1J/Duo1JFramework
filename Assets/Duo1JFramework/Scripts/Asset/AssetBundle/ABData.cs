using Duo1JFramework.TimerUpdate;
using System;
using System.Collections.Generic;
using UnityEngine;

using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// AssetBundle数据
    /// </summary>
    public class ABData
    {
        /// <summary>
        /// 加载出来的AssetBundle包
        /// </summary>
        private AssetBundle assetBundle;

        /// <summary>
        /// 待加载的AssetBundle包名
        /// </summary>
        private string assetBundleName;

        /// <summary>
        /// 待加载的AssetBundle文件路径
        /// </summary>
        private string assetBundlePath;

        /// <summary>
        /// 是否异步加载中
        /// </summary>
        private bool loading = false;

        /// <summary>
        /// 异步加载完成回调
        /// </summary>
        private Action asyncLoadedCallback;

        /// <summary>
        /// 引用的AssetBundle的列表
        /// </summary>
        private List<ABData> refABList;

        /// <summary>
        /// 引用该AssetBundle的Set
        /// </summary>
        private HashSet<ABData> refThisABSet;

        /// <summary>
        /// 该AssetBundle加载出来的资源列表
        /// </summary>
        private Dictionary<string, ABAssetData> abAssetDataDict;

        /// <summary>
        /// 卸载空闲等待时间
        /// </summary>
        private float freeTime = 0;

        public AssetBundle AB => assetBundle;

        public ABData(string assetBundleName)
        {
            this.assetBundleName = assetBundleName;
            assetBundlePath = Path.GetAssetBundlePath(assetBundleName);

            refABList = ABManager.Instance.GetRefABDataList(assetBundleName);
            refThisABSet = new HashSet<ABData>();
            abAssetDataDict = new Dictionary<string, ABAssetData>();
        }

        /// <summary>
        /// 异步加载资源
        /// </summary>
        public void Load<T>(string assetPath, Action<T> callback) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");
            Assert.NotNull(callback, "回调不可为空");

            CheckABLoaded(false, () =>
            {
                ABAssetData abAssetData = GetABAssetData(assetPath);
                abAssetData.Load<T>(callback);
            });
        }

        /// <summary>
        /// 同步加载资源
        /// </summary>
        public T LoadSync<T>(string assetPath) where T : UObject
        {
            Assert.NotNullOrEmpty(assetPath, "资源路径不可为空");

            CheckABLoaded(true);
            ABAssetData abAssetData = GetABAssetData(assetPath);
            return abAssetData.LoadSync<T>();
        }

        /// <summary>
        /// 卸载资源
        /// </summary>
        public void UnloadAsset(string assetPath)
        {
            if (!abAssetDataDict.TryGetValue(assetPath, out ABAssetData abAssetData))
            {
                Log.ErrorForce($"{ToString()} 未加载{assetPath}，无法卸载");
                return;
            }

            abAssetData.RemoveRef();
        }

        /// <summary>
        /// 是否可以卸载
        /// </summary>
        public bool CanUnload()
        {
            if (loading)
            {
                return false;
            }

            if (refThisABSet.Count > 0)
            {
                return false;
            }

            foreach (KeyValuePair<string, ABAssetData> kv in abAssetDataDict)
            {
                if (!kv.Value.CanUnload())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 尝试卸载
        /// </summary>
        public bool TryUnload()
        {
            return Unload(false);
        }

        /// <summary>
        /// 强制卸载
        /// </summary>
        public void ForceUnload()
        {
            Unload(true);
        }

        /// <summary>
        /// 卸载
        /// </summary>
        private bool Unload(bool force = false)
        {
            if (force || CanUnload())
            {
                loading = false;
                asyncLoadedCallback = null;

                foreach (KeyValuePair<string, ABAssetData> kv in abAssetDataDict)
                {
                    kv.Value.Unload(force);
                }

                foreach (ABData abData in refABList)
                {
                    abData.RemoveRefThis(this);
                }

                assetBundle.Unload(force);
                assetBundle = null;

                return true;
            }

            return false;
        }

        private ABAssetData GetABAssetData(string assetPath)
        {
            if (!IsABLoaded())
            {
                Log.ErrorForce($"{ToString()} 调用ABData::GetABAssetData()时，AssetBundle尚未加载完成");
                return null;
            }

            if (!abAssetDataDict.TryGetValue(assetPath, out ABAssetData abAssetData))
            {
                abAssetData = new ABAssetData(this, assetPath);
                abAssetDataDict.Add(assetPath, abAssetData);
            }
            return abAssetData;
        }

        /// <summary>
        /// 对此AssetBundle添加其他包的依赖
        /// </summary>
        private void AddRefThis(ABData abData)
        {
            refThisABSet.Add(abData);
        }

        /// <summary>
        /// 移除AssetBundle对此的依赖
        /// </summary>
        private void RemoveRefThis(ABData abData)
        {
            refThisABSet.Remove(abData);
        }

        /// <summary>
        /// 检查AssetBundle是否加载，未加载则加载
        /// </summary>
        private bool CheckABLoaded(bool sync, Action callback = null)
        {
            if (IsABLoaded())
            {
                callback?.Invoke();
                return true;
            }

            LoadAssetBundle(sync, callback);
            return false;

        }

        /// <summary>
        /// AssetBundle是否已加载
        /// </summary>
        public bool IsABLoaded()
        {
            return assetBundle != null;
        }

        /// <summary>
        /// 加载AssetBundle
        /// </summary>
        public void LoadAssetBundle(bool sync = false, Action callback = null)
        {
            if (IsABLoaded())
            {
                Log.Warn($"{ToString()} AssetBundle已加载，不可重复加载");
                callback?.Invoke();
                return;
            }

            if (sync)
            {
                InnerLoadAssetBundleSync(callback);
            }
            else
            {
                InnerLoadAssetBundle(callback);
            }
        }

        /// <summary>
        /// 内部异步加载AssetBundle
        /// </summary>
        private void InnerLoadAssetBundle(Action callback)
        {
            if (callback != null)
            {
                asyncLoadedCallback += callback;
            }

            if (loading)
            {
                return;
            }

            LoadAllDependenciesAB(false, () =>
            {
                loading = true;
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath);
                UpdateManager.Instance.RegisterAsyncRequest(request, (req) =>
                {
                    AssetBundleCreateRequest _request = req as AssetBundleCreateRequest;
                    AssetBundle _assetBundle = _request.assetBundle;

                    loading = false;

                    if (_assetBundle != null)
                    {
                        if (this.assetBundle != null)
                        {
                            this.assetBundle = _assetBundle;
                        }
                        else
                        {
                            Log.Warn($"{ToString()} AssetBundle已加载，抛弃本次异步结果");
                            _assetBundle.DestroyImmediate();
                        }
                    }
                    else
                    {
                        Log.ErrorForce($"{ToString()} 异步加载AssetBundle失败");
                    }

                    if (asyncLoadedCallback != null)
                    {
                        asyncLoadedCallback.Invoke();
                        asyncLoadedCallback = null;
                    }
                });
            });
        }

        /// <summary>
        /// 内部同步加载AssetBundle
        /// </summary>
        private void InnerLoadAssetBundleSync(Action callback)
        {
            LoadAllDependenciesAB(true, () =>
            {
                assetBundle = AssetBundle.LoadFromFile(assetBundlePath);
                if (assetBundle == null)
                {
                    Log.ErrorForce($"{ToString()} 同步加载AssetBundle失败");
                }

                callback?.Invoke();
            });
        }

        /// <summary>
        /// 加载所有依赖AssetBundle
        /// </summary>
        private void LoadAllDependenciesAB(bool sync, Action callback)
        {
            if (refABList != null)
            {
                int loadedCnt = 0;
                int allCnt = refABList.Count;

                foreach (ABData abData in refABList)
                {
                    abData.AddRefThis(this);

                    if (abData.IsABLoaded())
                    {
                        if (++loadedCnt == allCnt)
                        {
                            callback?.Invoke();
                            callback = null;
                        }
                    }
                    else
                    {
                        abData.LoadAssetBundle(sync, () =>
                        {
                            if (++loadedCnt == allCnt)
                            {
                                callback?.Invoke();
                                callback = null;
                            }
                        });
                    }
                }
            }
            else
            {
                callback?.Invoke();
                callback = null;
            }
        }

        public void Tick()
        {
            if (!IsABLoaded())
            {
                return;
            }

            if (!CanUnload())
            {
                freeTime = 0;
                return;
            }

            freeTime += Time.deltaTime;
            if (freeTime > Def.MAX_AB_FREE_TIME)
            {
                TryUnload();
            }
        }

        public override string ToString()
        {
            if (loading)
            {
                return $"<{assetBundleName}-Loading>";
            }
            else
            {
                return $"<{assetBundleName}>";
            }
        }
    }
}
