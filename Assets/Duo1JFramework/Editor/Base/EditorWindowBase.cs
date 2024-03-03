using UnityEditor;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器窗口基类
    /// </summary>
    public abstract class EditorWindowBase<T> : EditorWindow where T : EditorWindow
    {
        public float X => position.x;
        public float Y => position.y;
        public float Width => position.width;
        public float Height => position.height;

        /// <summary>
        /// 打开编辑器窗口
        /// </summary>
        /// <param name="wndName">窗口名，不填则使用配置的名称</param>
        public static T Open(string wndName = null)
        {
            return EditorUtil.OpenEditorWnd<T>(wndName);
        }

        #region Override

        protected virtual void LoadData()
        {
        }

        protected virtual void SaveData()
        {
        }

        #endregion Override

        #region Lifecycle

        protected virtual void OnEnable()
        {
            LoadData();
        }

        protected virtual void OnDisable()
        {
            SaveData();
        }

        #endregion Lifecycle
    }
}
