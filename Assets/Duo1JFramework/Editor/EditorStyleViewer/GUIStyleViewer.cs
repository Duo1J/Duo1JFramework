using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Duo1JFramework
{
    /// <summary>
    /// 内置样式列表
    /// </summary>
    public class GUIStyleViewer : BaseEditorWindow<GUIStyleViewer>
    {
        private struct DrawItem
        {
            public Rect rect;
            public Action action;
        }

        private List<DrawItem> drawItemList;
        private List<UnityEngine.Object> objList;

        private float scrollBarPos;
        private float maxY;
        private Rect oldPos;

        private bool showStyle = true;
        private bool showIcon = false;

        private string searchKey = "";

        private void OnGUI()
        {
            if (position.width != oldPos.width && UnityEngine.Event.current.type == EventType.Layout)
            {
                drawItemList = null;
                oldPos = position;
            }

            LU.Horizontal(() =>
            {
                if (GUILayout.Toggle(showStyle, "Styles", EditorStyles.toolbarButton) != showStyle)
                {
                    showStyle = !showStyle;
                    showIcon = !showStyle;
                    drawItemList = null;
                }

                if (GUILayout.Toggle(showIcon, "Icons", EditorStyles.toolbarButton) != showIcon)
                {
                    showIcon = !showIcon;
                    showStyle = !showIcon;
                    drawItemList = null;
                }
            });

            string newSearchKey = EditorGUILayout.TextField("Search", searchKey, "SearchTextField");
            if (newSearchKey != searchKey)
            {
                searchKey = newSearchKey;
                drawItemList = null;
            }

            float top = 36;

            if (drawItemList == null)
            {
                string lowerSearchKey = searchKey.ToLower();

                drawItemList = new List<DrawItem>();

                GUIContent inactiveText = new GUIContent("inactive");
                GUIContent activeText = new GUIContent("active");

                float x = 5.0f;
                float y = 5.0f;

                if (showStyle)
                {
                    foreach (GUIStyle s in GUI.skin.customStyles)
                    {
                        if (lowerSearchKey != "" && !s.name.ToLower().Contains(lowerSearchKey))
                            continue;

                        GUIStyle thisStyle = s;

                        DrawItem draw = new DrawItem();

                        float width = Mathf.Max(
                            100.0f,
                            GUI.skin.button.CalcSize(new GUIContent(s.name)).x,
                            s.CalcSize(inactiveText).x + s.CalcSize(activeText).x) + 16.0f;

                        float height = 60.0f;

                        if (x + width > position.width - 32 && x > 5.0f)
                        {
                            x = 5.0f;
                            y += height + 10.0f;
                        }

                        draw.rect = new Rect(x, y, width, height);

                        width -= 8.0f;

                        draw.action = () =>
                        {
                            if (GUILayout.Button(thisStyle.name, GUILayout.Width(width)))
                                EditorUtil.CopyText("\"" + thisStyle.name + "\"");

                            LU.Horizontal(() =>
                            {
                                GUILayout.Toggle(false, inactiveText, thisStyle, GUILayout.Width(width / 2));
                                GUILayout.Toggle(true, activeText, thisStyle, GUILayout.Width(width / 2));
                            });
                        };

                        x += width + 18.0f;

                        drawItemList.Add(draw);
                    }
                }
                else if (showIcon)
                {
                    if (objList == null)
                    {
                        objList = new List<UnityEngine.Object>(Resources.FindObjectsOfTypeAll(typeof(Texture)));
                        objList.Sort((pA, pB) => String.Compare(pA.name, pB.name, System.StringComparison.OrdinalIgnoreCase));
                    }

                    float rowHeight = 0.0f;

                    foreach (UnityEngine.Object o in objList)
                    {
                        Texture texture = (Texture)o;

                        if (texture.name == "")
                            continue;

                        if (texture is Texture2D == false)
                        {
                            continue;
                        }

                        if (lowerSearchKey != "" && !texture.name.ToLower().Contains(lowerSearchKey))
                            continue;

                        DrawItem draw = new DrawItem();

                        float width = Mathf.Max(
                            GUI.skin.button.CalcSize(new GUIContent(texture.name)).x,
                            texture.width) + 8.0f;

                        float height = texture.height + GUI.skin.button.CalcSize(new GUIContent(texture.name)).y + 8.0f;

                        if (x + width > position.width - 32.0f)
                        {
                            x = 5.0f;
                            y += rowHeight + 8.0f;
                            rowHeight = 0.0f;
                        }

                        draw.rect = new Rect(x, y, width, height);

                        rowHeight = Mathf.Max(rowHeight, height);

                        width -= 8.0f;

                        draw.action = () =>
                        {
                            if (GUILayout.Button(texture.name, GUILayout.Width(width)))
                                EditorUtil.CopyText("EditorGUIUtility.FindTexture( \"" + texture.name + "\" )");

                            Rect textureRect = GUILayoutUtility.GetRect(texture.width, texture.width, texture.height, texture.height, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));
                            EditorGUI.DrawTextureTransparent(textureRect, texture);
                        };

                        x += width + 8.0f;

                        drawItemList.Add(draw);
                    }
                }

                maxY = y;
            }

            Rect r = position;
            r.y = top;
            r.height -= r.y;
            r.x = r.width - 16;
            r.width = 16;

            float areaHeight = position.height - top;
            scrollBarPos = GUI.VerticalScrollbar(r, scrollBarPos, areaHeight, 0.0f, maxY);

            LU.Area(new Rect(0, top, position.width - 16.0f, areaHeight), () =>
            {
                int count = 0;
                foreach (DrawItem draw in drawItemList)
                {
                    Rect newRect = draw.rect;
                    newRect.y -= scrollBarPos;

                    if (newRect.y + newRect.height > 0 && newRect.y < areaHeight)
                    {
                        LU.Area(newRect, () =>
                        {
                            draw.action();
                        }, GUI.skin.textField);

                        count++;
                    }
                }
            });
        }
    }
}