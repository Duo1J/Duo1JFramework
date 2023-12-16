using UnityEditor;

namespace Duo1JFramework.UI
{
    public class UIAddComMenu
    {
        private const string Prefix = "GameObject/UI 扩展/";

        [MenuItem(Prefix + "Image", priority = 1)]
        public static void AddImage()
        {
            EditorUtil.GetActiveGo()?.AddImage();
        }

        [MenuItem(Prefix + "EmptyGraphic", priority = 2)]
        public static void AddEmptyGraphic()
        {
            EditorUtil.GetActiveGo()?.AddEmptyGraphic();
        }

        [MenuItem(Prefix + "RawImage", priority = 3)]
        public static void AddRawImage()
        {
            EditorUtil.GetActiveGo()?.AddRawImage();
        }

        [MenuItem(Prefix + "Button", priority = 4)]
        public static void AddButton()
        {
            EditorUtil.GetActiveGo()?.AddButton();
        }

        [MenuItem(Prefix + "TMP", priority = 5)]
        public static void AddTMP()
        {
            EditorUtil.GetActiveGo()?.AddTMP();
        }

        [MenuItem(Prefix + "Text", priority = 6)]
        public static void AddText()
        {
            EditorUtil.GetActiveGo()?.AddText();
        }
    }
}