using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Duo1JFramework.Pattern.Pipeline;

namespace Duo1JFramework.Build
{
    /// <summary>
    /// AssetBundle构建器
    /// </summary>
    public class AssetBundleBuilder
    {
        /// <summary>
        /// 使用ABBuildStrategy配置构建所有AssetBundle
        /// </summary>
        /// <see cref="ABBuildStrategy"/>
        public static EPipelineState BuildAllAssetBundle()
        {
            return BuildAllAssetBundle(ABBuildStrategy.Instance.BuildTarget, ABBuildStrategy.Instance.PipelineType);
        }

        /// <summary>
        /// 构建所有AssetBundle
        /// </summary>
        public static EPipelineState BuildAllAssetBundle(BuildTarget buildTarget, EABPipelineType pipelineType)
        {
            try
            {
                EPipelineState state = EPipelineState.Fail;

                switch (pipelineType)
                {
                    case EABPipelineType.Builtin:
                        ABBuiltinPipelineContext context = new ABBuiltinPipelineContext(buildTarget);
                        state = ABBuiltinPipeline.Build(context);
                        break;
                    default:
                        Log.EditorError($"未处理的AssetBundle管线类型: `{pipelineType}`");
                        state = EPipelineState.Fail;
                        break;
                }

                switch (state)
                {
                    case EPipelineState.Success:
                        Log.EditorInfo($"构建 `{buildTarget.GetName()}` 平台的AssetBundle成功");
                        break;
                    case EPipelineState.Fail:
                        Log.EditorError($"构建 `{buildTarget.GetName()}` 平台的AssetBundle失败");
                        break;
                    case EPipelineState.Break:
                        Log.EditorError($"构建 `{buildTarget.GetName()}` 平台的AssetBundle中断");
                        break;
                    default:
                        break;
                }

                return state;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"构建 `{buildTarget.GetName()}` 平台的AssetBundle异常");
                return EPipelineState.Fail;
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
                if (!strategyData.CheckValid())
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

                string abPath = PathUtil.GetAssetBundlePath(buildData.ABName, true);
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

                string abPath = PathUtil.GetAssetBundlePath(buildData.ABName, true);
                if (!File.Exists(abPath))
                {
                    Log.EditorError($"构建Hash时未找到AssetBundle文件: `{abPath}`");
                    ab2HashMap.Add(buildData.ABName, "");
                    continue;
                }

                if (BuildPipeline.GetHashForAssetBundle(abPath, out Hash128 hash))
                {
                    string hashStr = hash.ToString();

                    if (ab2HashMap.ContainsValue(hashStr))
                    {
                        Log.EditorError($"构建Hash重复, AssetBundle: `{abPath}`, Hash: `{hashStr}`");
                        continue;
                    }

                    ab2HashMap.Add(buildData.ABName, hashStr);
                }
                else
                {
                    Log.EditorError($"构建Hash失败, AssetBundle: `{abPath}`");
                    ab2HashMap.Add(buildData.ABName, "");
                }
            }

            return ab2HashMap;
        }

        /// <summary>
        /// 构建AssetBundle与MD5映射
        /// </summary>
        public static Dictionary<string, string> BuildAB2MD5Map(ABBuildData[] buildDatas)
        {
            Dictionary<string, string> ab2MD5Map = new Dictionary<string, string>();

            foreach (ABBuildData buildData in buildDatas)
            {
                if (buildData.IsEmpty())
                {
                    continue;
                }

                string abPath = PathUtil.GetAssetBundlePath(buildData.ABName, true);
                if (!File.Exists(abPath))
                {
                    Log.EditorError($"构建MD5时未找到AssetBundle文件: `{abPath}`");
                    continue;
                }

                try
                {
                    using (Stream stream = File.OpenRead(abPath))
                    {
                        string md5Str = CryptoUtil.MD5ComputeHashStr(stream);

                        if (string.IsNullOrEmpty(md5Str))
                        {
                            Log.EditorError($"构建MD5异常为空, AssetBundle: `{abPath}`");
                            continue;
                        }

                        if (ab2MD5Map.ContainsValue(md5Str))
                        {
                            Log.EditorError($"构建MD5重复, AssetBundle: `{abPath}`, MD5: `{md5Str}`");
                            continue;
                        }

                        ab2MD5Map.Add(buildData.ABName, md5Str);
                    }
                }
                catch (Exception e)
                {
                    Assert.ExceptHandle(e, $"构建MD5失败, AssetBundle: `{abPath}`");
                }
            }

            return ab2MD5Map;
        }

        /// <summary>
        /// 使用映射表重命名AssetBundle并添加后缀
        /// </summary>
        /// <param name="ab2NameMap">ab与名称映射, 若为空则使用原名</param>
        public static bool RenameAllAssetBundle(ABBuildData[] buildDatas, Dictionary<string, string> ab2NameMap = null)
        {
            string mainABPath = PathUtil.GetAssetBundlePath(Def.Path.ASSET_BUNDLE_MAIN_NAME, true);
            if (!File.Exists(mainABPath))
            {
                Log.EditorError($"重命名AssetBundle时未找到主包: `{mainABPath}`");
                return false;
            }

            string mainABRenamePath = PathUtil.GetAssetBundlePath(Def.Path.ASSET_BUNDLE_MAIN_NAME, false);
            FileUtil.Move(mainABPath, mainABRenamePath);

            foreach (ABBuildData buildData in buildDatas)
            {
                if (buildData.IsEmpty())
                {
                    continue;
                }

                string abPath = PathUtil.GetAssetBundlePath(buildData.ABName, true);
                if (!File.Exists(abPath))
                {
                    Log.EditorError($"重命名AssetBundle时未找到目标文件: `{abPath}`");
                    return false;
                }

                if (ab2NameMap == null)
                {
                    string renamePath = PathUtil.GetAssetBundlePath(buildData.ABName, false);
                    FileUtil.Move(abPath, renamePath);
                }
                else
                {
                    if (ab2NameMap.TryGetValue(buildData.ABName, out string name))
                    {
                        string renamePath = PathUtil.GetAssetBundlePath(name, false);
                        FileUtil.Move(abPath, renamePath);
                    }
                    else
                    {
                        Log.EditorError($"重命名AssetBundle时映射表中未找到名称, AB: `{abPath}`");
                        return false;
                    }
                }
            }

            return true;
        }

        private AssetBundleBuilder()
        {
        }
    }
}
