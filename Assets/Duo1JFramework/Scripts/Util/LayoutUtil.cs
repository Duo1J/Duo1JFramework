using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

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

        public static void Vertical(Action action, GUIStyle style)
        {
            GUILayout.BeginVertical(style);
            action?.Invoke();
            GUILayout.EndVertical();
        }

        public static void Horizontal(Action action, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
            action?.Invoke();
            GUILayout.EndHorizontal();
        }

        public static void Horizontal(Action action, GUIStyle style)
        {
            GUILayout.BeginHorizontal(style);
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

        public static Vector2 Scroll(Vector2 scrollPos, Action action, GUIStyle style)
        {
            Vector2 ret = GUILayout.BeginScrollView(scrollPos, style);
            action?.Invoke();
            GUILayout.EndScrollView();
            return ret;
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