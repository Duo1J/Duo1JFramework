using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// ±‡º≠∆˜≈‰÷√ScriptableObject
    /// </summary>
    public class EditorConfigSO<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;

        /// <summary>
        /// ±‡º≠∆˜≈‰÷√ µ¿˝
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
        /// ±‡º≠∆˜≈‰÷√ µ¿˝±£¥Ê¬∑æ∂
        /// </summary>
        public static string Path => EditorUtil.GetEditorCfgSOPath<T>();

        /// <summary>
        /// —°÷–≈‰÷√◊ ‘¥
        /// </summary>
        public static void SelectAsset()
        {
            ProjectViewUtil.SelectProjectAsset(Path);
        }
    }
}
