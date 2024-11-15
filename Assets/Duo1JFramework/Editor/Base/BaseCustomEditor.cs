using UnityEngine;
using Object = UnityEngine.Object;

namespace Duo1JFramework
{
    /// <summary>
    /// 自定义物体面板基类
    /// </summary>
    public abstract class BaseCustomEditor<T> : BaseEditor where T : Object
    {
        /// <summary>
        /// 目标实例
        /// </summary>
        protected T instance;

        /// <summary>
        /// 显示原始面板
        /// </summary>
        private bool showOrigin;

        /// <summary>
        /// 默认显示原始面板
        /// </summary>
        protected virtual bool ShowOriginDefault => false;

        protected virtual void OnEnable()
        {
            instance = Target<T>();
            showOrigin = ShowOriginDefault;
        }

        public override void OnInspectorGUI()
        {
            showOrigin = GUILayout.Toggle(showOrigin, "显示原始面板");
            GUILayout.Space(5);

            if (showOrigin)
            {
                base.OnInspectorGUI();
            }
            else
            {
                serializedObject.Update();
                DrawInspector();
                serializedObject.ApplyModifiedProperties();
            }
        }

        /// <summary>
        /// 子面板绘制Inspector
        /// </summary>
        protected abstract void DrawInspector();
    }
}
