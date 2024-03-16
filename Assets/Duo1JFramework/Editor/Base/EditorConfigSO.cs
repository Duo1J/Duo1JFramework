using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器配置ScriptableObject
    /// </summary>
    public class EditorConfigSO<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;

        /// <summary>
        /// 编辑器配置实例
        /// </summary>
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = EditorUtil.GetOrCreateEditorCfgSO<T>();
                }
                return instance;
            }
        }

        /// <summary>
        /// 编辑器配置实例保存路径
        /// </summary>
        public static string AssetPath => EditorUtil.GetEditorCfgSOPath<T>();

        /// <summary>
        /// 选中配置资源
        /// </summary>
        public static void SelectAsset()
        {
            ProjectViewUtil.SelectProjectAsset(AssetPath);
        }
    }
}
