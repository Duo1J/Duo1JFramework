using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace Duo1JFramework.Asset
{
    /// <summary>
    /// 资源导入管理器
    /// </summary>
    public static class AssetImportManager
    {
        private static readonly List<BaseAssetImportProcessor> processors = new List<BaseAssetImportProcessor>();

        static AssetImportManager()
        {
            RegisterAllProcessors();
        }

        /// <summary>
        /// 处理资源导入
        /// </summary>
        public static void Process(AssetImporter importer, string assetPath)
        {
            if (importer == null || string.IsNullOrEmpty(assetPath))
            {
                return;
            }

            foreach (BaseAssetImportProcessor processor in processors.Where(processor => processor.CanProcess(importer, assetPath)))
            {
                processor.Process(importer, assetPath);
            }
        }

        private static void RegisterAllProcessors()
        {
            processors.Clear();

            foreach (Type type in TypeCache.GetTypesDerivedFrom<BaseAssetImportProcessor>())
            {
                if (type.IsAbstract)
                {
                    continue;
                }

                if (Activator.CreateInstance(type) is BaseAssetImportProcessor processor)
                {
                    processors.Add(processor);
                }
            }

            processors.Sort((a, b) => a.Priority.CompareTo(b.Priority));
        }
    }
}
