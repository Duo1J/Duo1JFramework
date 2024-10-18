using Duo1JFramework.Build;
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
    public class ABData : IEditorDrawer
    {
        /// <summary>
        /// 加载出来的AssetBundle包
        /// </summary>
        private AssetBundle assetBundle;

        /// <summary>
        /// AssetBundle包名
        /// </summary>
        private string assetBundleName;

        /// <summary>
        /// Hash值
        /// </summary>
        private string hash;

        /// <summary>
        /// CRC校验码
        /// </summary>
        private uint crc;

        /// <summary>
        /// MD5值
        /// </summary>
        private string md5;

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
            hash = ABManager.Instance.GetHashStrByABName(assetBundleName);
            crc = ABManager.Instance.GetCRCByABName(assetBundleName);
            md5 = ABManager.Instance.GetMD5ByABName(assetBundleName);

            switch (Def.Asset.ABNameType)
            {
                case EABNameType.Origin:
                    assetBundlePath = PathUtil.GetAssetBundlePath(assetBundleName);
                    break;
                case EABNameType.Hash:
                    assetBundlePath = PathUtil.GetAssetBundlePath(hash);
                    break;
                case EABNameType.MD5:
                    assetBundlePath = PathUtil.GetAssetBundlePath(md5);
                    break;
                default:
                    assetBundlePath = PathUtil.GetAssetBundlePath(assetBundleName);
                    break;
            }

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
                if (abAssetData == null)
                {
                    callback?.Invoke(null);
                    return;
                }

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
            if (abAssetData == null)
            {
                return null;
            }

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

                if (assetBundle != null)
                {
                    assetBundle.Unload(force);
                    assetBundle = null;
                }

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
                AssetBundleCreateRequest request = AssetBundle.LoadFromFileAsync(assetBundlePath, crc);
                UpdateManager.Instance.RegisterAsyncRequest(request, (req) =>
                {
                    AssetBundleCreateRequest _request = req as AssetBundleCreateRequest;
                    AssetBundle _assetBundle = _request.assetBundle;

                    loading = false;

                    if (_assetBundle != null)
                    {
                        if (this.assetBundle == null)
                        {
                            this.assetBundle = _assetBundle;
                        }
                        else
                        {
                            Log.Warn($"{ToString()} AssetBundle已加载，抛弃本次异步结果");
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
            assetBundle = AssetBundle.LoadFromFile(assetBundlePath, crc);
            if (assetBundle == null)
            {
                Log.ErrorForce($"{ToString()} 同步加载AssetBundle失败");
            }

            LoadAllDependenciesAB(true, () =>
            {
                callback?.Invoke();
            });
        }

        /// <summary>
        /// 加载所有依赖AssetBundle
        /// </summary>
        private void LoadAllDependenciesAB(bool sync, Action callback)
        {
            if (refABList != null && refABList.Count != 0)
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
            if (freeTime > Def.Asset.MAX_AB_FREE_TIME)
            {
                TryUnload();
            }
        }

        public override string ToString()
        {
            if (loading)
            {
                return $"<{assetBundlePath}-Loading>";
            }
            else
            {
                return $"<{assetBundlePath}>";
            }
        }


        private bool drawRefABList = false;
        private bool drawRefThisABSet = false;
        private bool drawABAssetDataDict = false;
        public void DrawEditorInfo()
        {
            ED.Vertical(() =>
            {
                GUILayout.Label($"AB包名: {assetBundleName.WithColor(ES.Blue)}");
                GUILayout.Label($"AB路径: {assetBundlePath}");
                GUILayout.Label($"加载中: {loading}{ED.S4}卸载空闲等待时间: {freeTime}{ED.S4}是否可以卸载: {CanUnload()}");

                GUILayout.Space(10);

                if (ED.Toggle(ref drawRefABList, "显示引用的AssetBundle的列表".WithColor(ES.Green)))
                {
                    ED.Vertical(() =>
                    {
                        if (refABList == null || refABList.Count == 0)
                        {
                            GUILayout.Label("refABList为空");
                        }
                        else
                        {
                            foreach (ABData abData in refABList)
                            {
                                GUILayout.Label(abData.ToString());
                            }
                        }
                    }, "box");

                    GUILayout.Space(10);
                }

                if (ED.Toggle(ref drawRefThisABSet, "显示引用该AssetBundle的Set".WithColor(ES.Green)))
                {
                    ED.Vertical(() =>
                    {
                        if (refThisABSet == null || refThisABSet.Count == 0)
                        {
                            GUILayout.Label("refThisABSet为空");
                        }
                        else
                        {
                            foreach (ABData abData in refThisABSet)
                            {
                                GUILayout.Label(abData.ToString());
                            }
                        }
                    }, "box");

                    GUILayout.Space(10);
                }

                if (ED.Toggle(ref drawABAssetDataDict, "显示该AssetBundle加载出来的资源列表".WithColor(ES.Green)))
                {
                    ED.Vertical(() =>
                    {
                        if (abAssetDataDict == null || abAssetDataDict.Count == 0)
                        {
                            GUILayout.Label("abAssetDataDict为空");
                        }
                        else
                        {
                            GUILayout.Space(5);
                            foreach (KeyValuePair<string, ABAssetData> kv in abAssetDataDict)
                            {
                                kv.Value.DrawEditorInfo();
                            }
                        }
                    }, "box");
                }
            });
        }
    }
}
