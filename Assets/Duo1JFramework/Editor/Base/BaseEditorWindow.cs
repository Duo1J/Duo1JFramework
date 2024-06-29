using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// 创建新的窗口后需要配置窗口名
    /// <see cref="EditorDef.Menu.editorWndNameMap"/>
    /// 需要添加到菜单
    /// <see cref="ToolMenuConfig"/>


    /// <summary>
    /// 编辑器窗口基类
    /// </summary>
    public abstract class BaseEditorWindow<T> : EditorWindow where T : EditorWindow
    {
        /// <summary>
        /// 窗口位置X
        /// </summary>
        public float X => position.x;

        /// <summary>
        /// 窗口位置Y
        /// </summary>
        public float Y => position.y;

        /// <summary>
        /// 窗口宽度
        /// </summary>
        public float Width => position.width;

        /// <summary>
        /// 窗口高度
        /// </summary>
        public float Height => position.height;

        /// <summary>
        /// 设置所有样式的富文本
        /// </summary>
        public static bool RichText
        {
            set
            {
                ES.SetRichText(value);
            }
        }

        /// <summary>
        /// 错误消息
        /// </summary>
        /// <see cref="SetErrMsg"/>
        /// <see cref="DrawErrMsg"/>
        private string errMsg;

        /// <summary>
        /// 打开编辑器窗口
        /// </summary>
        /// <param name="wndName">窗口名，不填则使用配置的名称</param>
        public static T Open(string wndName = null)
        {
            return EditorUtil.OpenEditorWnd<T>(wndName);
        }

        #region Override

        /// <summary>
        /// 加载数据
        /// </summary>
        protected virtual void LoadData()
        {
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        protected virtual void SaveData()
        {
        }

        #endregion Override

        #region Tool Function

        /// <summary>
        /// 设置错误消息
        /// </summary>
        protected void SetErrMsg(string errMsg)
        {
            this.errMsg = errMsg;
            if (!string.IsNullOrEmpty(errMsg))
            {
                Log.EditorError(errMsg);
            }
        }

        /// <summary>
        /// 绘制错误消息
        /// </summary>
        protected void DrawErrMsg()
        {
            if (string.IsNullOrEmpty(errMsg))
            {
                return;
            }

            if (EditorStyle.RichTextLabel)
            {
                GUILayout.Label($"<color=red>{errMsg}</color>");
            }
            else
            {
                GUILayout.Label(errMsg);
            }
        }

        #endregion Tool Function

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
