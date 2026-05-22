using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源检查管理器
    /// </summary>
    public static class AssetCheckManager
    {
        private static readonly List<BaseAssetChecker> checkers = new List<BaseAssetChecker>();

        static AssetCheckManager()
        {
            RegisterAllCheckers();
        }

        /// <summary>
        /// 检查资源
        /// </summary>
        public static AssetCheckResult Check(UObject asset, string assetPath)
        {
            BaseAssetChecker checker = GetChecker(asset, assetPath);
            if (checker == null)
            {
                AssetCheckResult result = new AssetCheckResult();
                result.AddError($"未找到资源类型`{asset?.GetType().Name}`对应的检查器");
                return result;
            }

            return checker.Check(asset, assetPath);
        }

        /// <summary>
        /// 获取资源检查器
        /// </summary>
        public static BaseAssetChecker GetChecker(UObject asset, string assetPath)
        {
            return checkers.FirstOrDefault(checker => checker.CanCheck(asset, assetPath));
        }

        private static void RegisterAllCheckers()
        {
            checkers.Clear();

            foreach (Type type in TypeCache.GetTypesDerivedFrom<BaseAssetChecker>())
            {
                if (type.IsAbstract)
                {
                    continue;
                }

                if (Activator.CreateInstance(type) is BaseAssetChecker checker)
                {
                    checkers.Add(checker);
                }
            }

            checkers.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }
    }
}
