using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Progress;

namespace Duo1JFramework
{
    /// <summary>
    /// 编辑器界面布局工具 (LayoutUtil)
    /// </summary>
    public class LU
    {
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

        public static void DisableGroup(Action action, bool disabled = true)
        {
            EditorGUI.BeginDisabledGroup(disabled);
            action?.Invoke();
            EditorGUI.EndDisabledGroup();
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

        protected LU()
        {
        }
    }

    public class LayoutUtil : LU
    {
        protected LayoutUtil()
        {
        }
    }
}