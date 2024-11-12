using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 字符串列表面板
    /// </summary>
    public class StringListPanel : BaseEditorWindow<StringListPanel>
    {
        /// <summary>
        /// 显示的字符串列表
        /// </summary>
        public List<string> StringList { get; set; }

        private Vector2 scrollPos;

        public static StringListPanel Open(List<string> stringList, string title = null)
        {
            StringListPanel panel = GetWindow<StringListPanel>();

            panel.titleContent.text = string.IsNullOrEmpty(title) ? "StringList" : title;
            panel.StringList = stringList;

            return panel;
        }

        public static StringListPanel Open(string[] stringList, string title = null)
        {
            return Open(new List<string>(stringList), title);
        }

        private void OnGUI()
        {
            RichText = true;
            DrawErrMsg();

            if (StringList == null)
            {
                SetErrMsg("字符串列表为空");
                return;
            }

            SetErrMsg(null);

            ED.Scroll(ref scrollPos, () =>
            {
                for (int i = 0; i < StringList.Count; i++)
                {
                    ED.Horizontal(() => { GUILayout.Label(StringList[i]); }, i % 2 == 0 ? "box" : GUIStyle.none);
                }
            });
        }
    }
}
