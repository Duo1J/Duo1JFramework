using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 引用工具
    /// </summary>
    public class ReferenceUtil
    {
        /// <summary>
        /// 打开目标引用资源列表
        /// </summary>
        public static void OpenReferenceAssetList(Object obj, bool recursive)
        {
            Assert.NotNullArg(obj, "obj");
            string assetPath = AssetDatabase.GetAssetPath(obj);
            string[] dependencies = AssetDatabase.GetDependencies(assetPath, recursive);
            StringListPanel.Open(dependencies, $"{obj.name} 引用资源列表");
        }
    }
}
