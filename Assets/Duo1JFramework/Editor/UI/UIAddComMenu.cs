using UnityEditor;
using UnityEngine;

namespace Duo1JFramework.UI
{
    public class UIAddComMenu
    {
        private const string Prefix = "GameObject/UI 扩展/";

        [MenuItem(Prefix + "Image", priority = 1)]
        public static void AddImage()
        {
            GameObject go = new GameObject("Image");
            go.AddImage();
            EditorUtil.SetParentToActiveGo(go);
            EditorUtil.SetActiveGo(go);
        }

        [MenuItem(Prefix + "EmptyGraphic", priority = 2)]
        public static void AddEmptyGraphic()
        {
            GameObject go = new GameObject("EmptyGraphic");
            go.AddEmptyGraphic();
            EditorUtil.SetParentToActiveGo(go);
            EditorUtil.SetActiveGo(go);
        }

        [MenuItem(Prefix + "RawImage", priority = 3)]
        public static void AddRawImage()
        {
            GameObject go = new GameObject("RawImage");
            go.AddRawImage();
            EditorUtil.SetParentToActiveGo(go);
            EditorUtil.SetActiveGo(go);
        }

        [MenuItem(Prefix + "Button", priority = 4)]
        public static void AddButton()
        {
            GameObject go = new GameObject("Button");
            go.AddButton();
            EditorUtil.SetParentToActiveGo(go);
            EditorUtil.SetActiveGo(go);
        }

        [MenuItem(Prefix + "TMP", priority = 5)]
        public static void AddTMP()
        {
            GameObject go = new GameObject("TMP");
            go.AddTMP();
            EditorUtil.SetParentToActiveGo(go);
            EditorUtil.SetActiveGo(go);
        }

        [MenuItem(Prefix + "Text", priority = 6)]
        public static void AddText()
        {
            GameObject go = new GameObject("Text");
            go.AddText();
            EditorUtil.SetParentToActiveGo(go);
            EditorUtil.SetActiveGo(go);
        }
    }
}