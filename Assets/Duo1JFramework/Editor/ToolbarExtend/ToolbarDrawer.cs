using System;
using System.Collections.Generic;
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

        private static List<DrawItem> leftBtnList = new List<DrawItem>();
        private static List<DrawItem> rightBtnList = new List<DrawItem>();

        /// <summary>
        /// 初始化左侧工具栏
        /// </summary>
        private static void InitLeftToolbar()
        {
        }

        /// <summary>
        /// 初始化右侧工具栏
        /// </summary>
        private static void InitRightToolbar()
        {
            rightBtnList.Add(new DrawItem(EditorGUIUtility.TrTextContentWithIcon("", "", "d_RotateTool On"), () =>
            {
                EditorUtil.SaveAndRefresh("Toolbar");
            }));

            rightBtnList.Add(new DrawItem(EditorGUIUtility.TrTextContentWithIcon("", "", "d_FolderEmpty Icon"), () =>
            {
                ProjectUtil.OpenExplorer(Def.Path.DataPath);
            }));
        }

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

            if (leftBtnList == null) leftBtnList = new List<DrawItem>();
            else leftBtnList.Clear();
            InitLeftToolbar();

            if (rightBtnList == null) rightBtnList = new List<DrawItem>();
            else rightBtnList.Clear();
            InitRightToolbar();
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
