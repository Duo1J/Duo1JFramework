using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 自定义Inspector面板基类
    /// </summary>
    public abstract class BaseCustomEditor<T> : BaseEditor where T : MonoBehaviour
    {
        /// <summary>
        /// 目标实例
        /// </summary>
        protected T instance;

        /// <summary>
        /// 显示原始面板
        /// </summary>
        private bool showOrigin;

        protected virtual void OnEnable()
        {
            instance = (T)target;
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
                Draw();
                serializedObject.ApplyModifiedProperties();
            }
        }

        protected abstract void Draw();
    }
}