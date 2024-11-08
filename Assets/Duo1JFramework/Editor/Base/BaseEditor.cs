using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// Editor基类
    /// </summary>
    public abstract class BaseEditor : Editor
    {
        /// <summary>
        /// Editor绑定目标
        /// </summary>
        public T Target<T>() where T : Object
        {
            return target.Convert<T>();
        }

        /// <summary>
        /// 设置所有样式的富文本
        /// </summary>
        public static bool RichText
        {
            set { ES.SetRichText(value); }
        }
    }
}