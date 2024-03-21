using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.VisualScripting;
using UnityEditor;

namespace Duo1JFramework
{
    /// <summary>
    /// 内置图标列表
    /// </summary>
    public class GUIIconViewer : EditorWindowBase<GUIIconViewer>
    {
        private List<GUIContent> iconList;
        private Vector2 scrollPos;
        private float cellSize = 35;

        private void DrawIconList()
        {
            LU.Scroll(ref scrollPos, () =>
            {
                int col = Mathf.FloorToInt(Width / cellSize);
                for (int i = 0; i < iconList.Count; i += col)
                {
                    LU.Horizontal(() =>
                    {
                        for (int j = 0; j < col; j++)
                        {
                            int idx = i + j;
                            if (idx < iconList.Count)
                            {
                                GUIContent content = iconList[idx];
                                if (GUILayout.Button(iconList[idx], GUILayout.Width(cellSize), GUILayout.Height(cellSize)))
                                {
                                    EditorUtil.CopyText(content.image.name);
                                }
                            }
                        }
                    });
                }
            });
        }

        private void OnGUI()
        {
            if (iconList == null)
            {
                iconList = new List<GUIContent>();

                Texture2D[] textures = Resources.FindObjectsOfTypeAll<Texture2D>();
                foreach (Texture2D texture in textures)
                {
                    try
                    {
                        GUIContent icon = EditorGUIUtility.IconContent(texture.name, $"|{texture.name}");
                        if (icon != null && icon.image != null)
                        {
                            iconList.Add(icon);
                        }
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            LU.Vertical(() =>
            {
                LU.Horizontal(() =>
                {
                    GUILayout.Label("大小");
                    cellSize = GUILayout.HorizontalSlider(cellSize, 35, 70);
                });
                DrawIconList();
            });
        }
    }
}
