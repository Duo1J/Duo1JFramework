using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器配置ScriptableObject
    /// </summary>
    public abstract class EditorConfigSO<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;

        /// <summary>
        /// 配置实例
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
        /// 配置实例保存路径
        /// </summary>
        public static string AssetPath => EditorUtil.GetEditorCfgSOPath<T>();

        /// <summary>
        /// 选中配置资源
        /// </summary>
        public void SelectAsset()
        {
            ProjectUtil.SelectProjectAsset(AssetPath);
        }

        /// <summary>
        /// 实例触发
        /// </summary>
        public void Trigger()
        {
        }
    }
}
