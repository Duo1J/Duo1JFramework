using System;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 工具栏扩展
    /// </summary>
    [InitializeOnLoad]
    public class ToolbarDrawer
    {
        private const string BtnStyle = "AppCommand";

        private static DrawItem[] leftBtnList = new DrawItem[]
        {
            new DrawItem(EditorGUIUtility.TrTextContentWithIcon("", "", "d_FolderEmpty Icon"), () =>
            {
                //todo hlj
            })
        };

        private static DrawItem[] rightBtnList = new DrawItem[]
        {
        };

        private static void OnLeftToolbarGUI()
        {
            foreach (DrawItem item in leftBtnList)
            {
                if (GUILayout.Button(item.content, BtnStyle))
                {
                    item.action();
                }
            }
        }

        private static void OnRightToolbarGUI()
        {
            foreach (DrawItem item in rightBtnList)
            {
                if (GUILayout.Button(item.content, BtnStyle))
                {
                    item.action();
                }
            }
        }

        static ToolbarDrawer()
        {
            ToolbarExtender.LeftToolbarGUI = OnLeftToolbarGUI;
            ToolbarExtender.RightToolbarGUI = OnRightToolbarGUI;
        }

        private struct DrawItem
        {
            public GUIContent content;
            public Action action;

            public DrawItem(GUIContent content, Action action)
            {
                this.content = content;
                this.action = action;
            }
        }
    }
}
