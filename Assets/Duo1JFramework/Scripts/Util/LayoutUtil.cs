using System;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器界面绘制工具
    /// </summary>
    public class LayoutUtil
    {
        public const string S2 = "  ";
        public const string S4 = "    ";
        public const string S6 = "      ";

        public static void Vertical(Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
            action?.Invoke();
            GUILayout.EndVertical();
        }

        public static void Vertical(Action action, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
            action?.Invoke();
            GUILayout.EndVertical();
        }

        public static void Horizontal(Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            action?.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void Horizontal(Action action, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
            action?.Invoke();
            GUILayout.EndHorizontal();
        }

        public static Vector2 Scroll(Vector2 scrollPos, Action action, params GUILayoutOption[] options)
        {
            Vector2 ret = GUILayout.BeginScrollView(scrollPos, options);
            action?.Invoke();
            GUILayout.EndScrollView();
            return ret;
        }

        public static void Scroll(ref Vector2 scrollPos, Action action, params GUILayoutOption[] options)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, options);
            action?.Invoke();
            GUILayout.EndScrollView();
        }

        public static Vector2 Scroll(Vector2 scrollPos, Action action, GUIStyle style, params GUILayoutOption[] options)
        {
            Vector2 ret = GUILayout.BeginScrollView(scrollPos, style, options);
            action?.Invoke();
            GUILayout.EndScrollView();
            return ret;
        }

        public static void Scroll(ref Vector2 scrollPos, Action action, GUIStyle style, params GUILayoutOption[] options)
        {
            scrollPos = GUILayout.BeginScrollView(scrollPos, style, options);
            action?.Invoke();
            GUILayout.EndScrollView();
        }

        public static void Area(Rect rect, Action action)
        {
            GUILayout.BeginArea(rect);
            action?.Invoke();
            GUILayout.EndArea();
        }

        public static void Area(Rect rect, Action action, GUIStyle style)
        {
            GUILayout.BeginArea(rect, style);
            action?.Invoke();
            GUILayout.EndArea();
        }

        public static void SurrondSpace(float space, Action action)
        {
            GUILayout.Space(space);
            action?.Invoke();
            GUILayout.Space(space);
        }

        public static void SurrondColor(Color color, Action action)
        {
            Color oriColor = GUI.color;
            GUI.color = color;
            action?.Invoke();
            GUI.color = oriColor;
        }

        public static void SurrondColor(bool con, Color color, Action action)
        {
            if (con)
            {
                SurrondColor(color, action);
            }
            else
            {
                action?.Invoke();
            }
        }

        /// <summary>
        /// 为上一个Rect下画分界线
        /// </summary>
        public static void LastSeparator()
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            GUILayout.Space(7);
            SurrondColor(new Color(0, 0, 0, 0.3f), () =>
            {
                GUI.DrawTexture(Rect.MinMaxRect(lastRect.xMin, lastRect.yMax + 4, lastRect.xMax, lastRect.yMax + 6), Texture2D.whiteTexture);
            });
        }

        /// <summary>
        /// 为上一个Rect下画粗分界线
        /// </summary>
        public static void LastBoldSeparator()
        {
            Rect lastRect = GUILayoutUtility.GetLastRect();
            GUILayout.Space(14);
            SurrondColor(new Color(0, 0, 0, 0.3f), () =>
            {
                GUI.DrawTexture(new Rect(0, lastRect.yMax + 6, Screen.width, 4), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(0, lastRect.yMax + 6, Screen.width, 1), Texture2D.whiteTexture);
                GUI.DrawTexture(new Rect(0, lastRect.yMax + 9, Screen.width, 1), Texture2D.whiteTexture);
            });
        }

        /// <summary>
        /// 为上一个TextField添加comment
        /// </summary>
        public static void LastCommentTextField(string checkContent, string comment = "Comments...")
        {
            if (!string.IsNullOrEmpty(checkContent))
            {
                return;
            }

            Rect lastRect = GUILayoutUtility.GetLastRect();
            GUI.Label(lastRect, " <i>" + comment + "</i>");
        }

        public static bool Toggle(ref bool toggle, string msg)
        {
            toggle = GUILayout.Toggle(toggle, msg);
            return toggle;
        }

        public static void HelpBox(string msg)
        {
#if UNITY_EDITOR
            EditorGUILayout.HelpBox(new GUIContent(msg));
#else
                GUILayout.Label(msg);
#endif
        }

        public static bool IsPlayingHelpBox()
        {
            return ConditionHelpBox(Game.IsPlaying, "请在运行后使用");
        }

        public static bool ConditionHelpBox(bool con, string msg)
        {
            if (!con)
            {
                HelpBox(msg);
            }
            return con;
        }

        #region Editor

        public static void DisableGroup_Editor(Action action, bool disabled = true)
        {
#if UNITY_EDITOR
            EditorGUI.BeginDisabledGroup(disabled);
            action?.Invoke();
            EditorGUI.EndDisabledGroup();
#else
            action?.Invoke();
#endif
        }

        #endregion Editor

        protected LayoutUtil()
        {
        }
    }

    public class LU : LayoutUtil
    {
        protected LU()
        {
        }
    }
}