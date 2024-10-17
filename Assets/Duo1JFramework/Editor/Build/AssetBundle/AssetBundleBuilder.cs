using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle构建器
    /// </summary>
    public class AssetBundleBuilder
    {
        /// <summary>
        /// 使用ABBuildStrategy配置的目标构建所有AssetBundle
        /// </summary>
        /// <see cref="ABBuildStrategy"/>
        public static void BuildAllAssetBundle()
        {
            BuildAllAssetBundle(ABBuildStrategy.Instance.BuildTarget);
        }

        /// <summary>
        /// 构建所有AssetBundle
        /// </summary>
        public static void BuildAllAssetBundle(BuildTarget buildTarget)
        {
            ClearAllAssetBundleBuild();

            ABBuildStrategyData[] strategyDatas = ABBuildStrategy.Instance.Data;

            if (strategyDatas == null || strategyDatas.Length == 0)
            {
                Log.EditorError($"AB构建策略配置为空: `{ABBuildStrategy.AssetPath}`");
                ABBuildStrategy.Instance.SelectAsset();
                return;
            }

            ABBuildData[] buildDatas = StrategyToBuildData(strategyDatas);

            if (buildDatas == null || buildDatas.Length == 0)
            {
                Log.EditorError($"AB构建数据为空，请检查策略配置: `{ABBuildStrategy.AssetPath}`");
                ABBuildStrategy.Instance.SelectAsset();
                return;
            }

            List<AssetBundleBuild> buildList = new List<AssetBundleBuild>();
            Dictionary<string, List<string>> ab2AssetMap = new Dictionary<string, List<string>>();

            foreach (ABBuildData buildData in buildDatas)
            {
                if (ab2AssetMap.ContainsKey(buildData.ABName))
                {
                    Log.EditorError($"AssetBundle包名重复: {buildData.ABName}");
                    continue;
                }
                ab2AssetMap.Add(buildData.ABName, buildData.AssetPathList);

                if (buildData.IsEmpty())
                {
                    continue;
                }

                buildList.Add(buildData.ToAssetBundleBuild());
            }

            try
            {
                EditorUtility.DisplayProgressBar("构建AssetBndle", "正在构建AssetBundle...", 0.3f);

                BuildPipeline.BuildAssetBundles(
                    PathUtil.GetAssetBundleEditorRoot().CheckDir(),
                    buildList.ToArray(),
                    ABBuildStrategy.Instance.BuildOptions,
                    buildTarget
                );

                Dictionary<string, uint> ab2CrcMap = null;
                if (Def.Asset.BuildABCRC)
                {
                    ab2CrcMap = BuildAB2CRCMap(buildDatas);
                }

                Dictionary<string, string> ab2HashMap = BuildAB2HashMap(buildDatas);

                ABMapData.SaveToFile(ab2AssetMap, ab2HashMap, ab2CrcMap, Def.Asset.EncryptABMapData);

                Log.EditorInfo($"构建{buildTarget.GetName()}平台的AssetBndle成功");
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, "AssetBundle构建异常");
            }
            finally
            {
                EditorUtility.ClearProgressBar();
                EditorUtil.SaveAndRefresh("AssetBundleBuilder::BuildAllAssetBundle");
            }
        }

        /// <summary>
        /// 清理所有构建的AssetBundle
        /// </summary>
        public static void ClearAllAssetBundleBuild()
        {
            FileUtil.DeleteDir(PathUtil.GetAssetBundleEditorRoot());
            EditorUtil.SaveAndRefresh("AssetBundleBuilder::ClearAllAssetBundleBuild");
        }

        /// <summary>
        /// 清理所有拷贝的运行时AssetBundle
        /// </summary>
        public static void ClearAllAssetBundleCopy()
        {
            FileUtil.DeleteDir(PathUtil.GetAssetBundleRuntimeRoot());
            FileUtil.DeleteFile(PathUtil.GetAssetBundleRuntimeRootMeta());
            EditorUtil.SaveAndRefresh("AssetBundleBuilder::ClearAllAssetBundleCopy");
        }

        /// <summary>
        /// 拷贝所有构建的AssetBundle到运行时文件夹
        /// </summary>
        public static bool CopyAllAssetBundleBuild()
        {
            try
            {
                AssetDatabase.StartAssetEditing();

                string editorRoot = PathUtil.GetAssetBundleEditorRoot();
                if (!Directory.Exists(editorRoot))
                {
                    Log.ErrorForce($"未找到AssetBundle的构建根文件夹，请重新构建: `{editorRoot}`");
                    return false;
                }

                ClearAllAssetBundleCopy();

                string runtimeRoot = PathUtil.GetAssetBundleRuntimeRoot();
                return FileUtil.CopyDirectory(editorRoot, runtimeRoot);
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, "拷贝所有构建的AssetBundle到运行时文件夹时异常");
                return false;
            }
            finally
            {
                AssetDatabase.StopAssetEditing();
                EditorUtil.SaveAndRefresh("AssetBundleBuilder::CopyAllAssetBundleBuild");
            }
        }

        /// <summary>
        /// 删除所有运行时文件夹拷贝的Manifest文件
        /// </summary>
        public static void DeleteAllManifestCopy()
        {
            EditorUtil.AssetEditing(() =>
            {
                try
                {
                    string runtimeRoot = PathUtil.GetAssetBundleRuntimeRoot();
                    List<string> fileList = FileUtil.GetFileInDir(runtimeRoot, null, $"*{Def.Path.MANIFEST_SUFFIX}");
                    List<string> metaFileList = FileUtil.GetFileInDir(runtimeRoot, null, $"*{Def.Path.MANIFEST_SUFFIX}{Def.Path.META_SUFFIX}");
                    fileList.ForEach(file => FileUtil.DeleteFile(file));
                    metaFileList.ForEach(file => FileUtil.DeleteFile(file));
                }
                catch (Exception e)
                {
                    Assert.ExceptHandle(e, "删除所有运行时文件夹拷贝的Manifest文件时异常");
                }
            }, "AssetBundleBuilder::DeleteAllManifestCopy");

            EditorUtil.SaveAndRefresh("AssetBundleBuilder::DeleteAllManifestCopy");
        }

        /// <summary>
        /// 命令行构建所有AssetBundle
        /// </summary>
        public static void CommandBuildAllAssetBundle()
        {

        }

        /// <summary>
        /// 将策略数据转为构建数据
        /// </summary>
        public static ABBuildData[] StrategyToBuildData(ABBuildStrategyData[] strategyDatas)
        {
            List<ABBuildData> ret = new List<ABBuildData>();

            if (strategyDatas == null || strategyDatas.Length == 0)
            {
                return ret.ToArray();
            }

            string pathPrefix = Def.Path.ASSET_FULL_PATH_PREFIX;
            foreach (ABBuildStrategyData strategyData in strategyDatas)
            {
                if (!strategyData.CheckValiad())
                {
                    Log.EditorError($"AB策略配置项无效，abName: {strategyData.abName}");
                    continue;
                }

                ABBuildData buildData = new ABBuildData(strategyData.abName);

                foreach (string path in strategyData.pathList)
                {
                    string _path = path.SplitUnify();
                    List<string> resultList = FileUtil.GetFileInDir(pathPrefix + _path, (p) =>
                    {
                        if (p.EndsWith(Def.Path.META_SUFFIX))
                        {
                            return null;
                        }

                        return p.Replace(pathPrefix, Def.Path.ASSET_PATH_PREFIX);
                    });

                    if (buildData.AssetPathList == null || buildData.AssetPathList.Count == 0)
                    {
                        buildData.AssetPathList = resultList;
                    }
                    else
                    {
                        List<string> assetPathList = buildData.AssetPathList;
                        foreach (string item in resultList)
                        {
                            assetPathList.Add(item);
                        }
                    }
                }

                ret.Add(buildData);
            }

            return ret.ToArray();
        }

        /// <summary>
        /// 构建AssetBundle与CRC映射
        /// </summary>
        public static Dictionary<string, uint> BuildAB2CRCMap(ABBuildData[] buildDatas)
        {
            Dictionary<string, uint> ab2CrcMap = new Dictionary<string, uint>();

            foreach (ABBuildData buildData in buildDatas)
            {
                if (buildData.IsEmpty())
                {
                    continue;
                }

                string abPath = PathUtil.GetAssetBundlePath(buildData.ABName);
                if (!File.Exists(abPath))
                {
                    Log.EditorError($"构建CRC时未找到AssetBundle文件: `{abPath}`");
                    ab2CrcMap.Add(buildData.ABName, 0);
                    continue;
                }

                if (BuildPipeline.GetCRCForAssetBundle(abPath, out uint crc))
                {
                    ab2CrcMap.Add(buildData.ABName, crc);
                }
                else
                {
                    Log.EditorError($"构建CRC失败, AssetBundle: `{abPath}`");
                    ab2CrcMap.Add(buildData.ABName, 0);
                }
            }

            return ab2CrcMap;
        }

        /// <summary>
        /// 构建AssetBundle与Hash映射
        /// </summary>
        public static Dictionary<string, string> BuildAB2HashMap(ABBuildData[] buildDatas)
        {
            Dictionary<string, string> ab2HashMap = new Dictionary<string, string>();

            foreach (ABBuildData buildData in buildDatas)
            {
                if (buildData.IsEmpty())
                {
                    continue;
                }

                string abPath = PathUtil.GetAssetBundlePath(buildData.ABName);
                if (!File.Exists(abPath))
                {
                    Log.EditorError($"构建Hash时未找到AssetBundle文件: `{abPath}`");
                    ab2HashMap.Add(buildData.ABName, "");
                    continue;
                }

                if (BuildPipeline.GetHashForAssetBundle(abPath, out Hash128 hash))
                {
                    ab2HashMap.Add(buildData.ABName, hash.ToString());
                }
                else
                {
                    Log.EditorError($"构建Hash失败, AssetBundle: `{abPath}`");
                    ab2HashMap.Add(buildData.ABName, "");
                }
            }

            return ab2HashMap;
        }

        private AssetBundleBuilder()
        {
        }
    }
}
