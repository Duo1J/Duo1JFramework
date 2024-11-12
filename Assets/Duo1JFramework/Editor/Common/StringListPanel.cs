using Duo1JFramework.ObjectPool;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
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

            if (StringList == null || StringList.Count == 0)
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

            GUILayout.FlexibleSpace();

            if (GUILayout.Button("复制"))
            {
                Copy();
            }

            if (GUILayout.Button("导出到文件"))
            {
                ExportToFile();
            }
        }

        private void Copy()
        {
            if (StringList == null || StringList.Count == 0)
            {
                Log.EditorError("字符串列表为空, 无法复制");
                return;
            }

            Pool.StringBuilderPool.Using((sb) =>
            {
                foreach (string str in StringList)
                {
                    sb.AppendLine(str);
                }

                EditorUtil.CopyText(sb.ToString());
            });
        }

        private void ExportToFile()
        {
            if (StringList == null || StringList.Count == 0)
            {
                Log.EditorError("字符串列表为空, 无法导出到文件");
                return;
            }

            string tarPath = EditorUtility.SaveFilePanel($"选择`{Title}`导出路径", null, titleContent.text, "txt");
            if (string.IsNullOrEmpty(tarPath))
            {
                return;
            }

            File.WriteAllLines(tarPath, StringList.ToArray());
            Log.EditorInfo($"`{Title}`导出到文件成功");
        }
    }
}
