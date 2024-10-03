using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI添加组件逻辑，不可重复挂载
    /// </summary>
    public static class UIAddComponent
    {
        public static ImageExt AddImage(this GameObject go)
        {
            ImageExt com = go.GetComponent<ImageExt>();
            if (com != null)
            {
                return com;
            }

            com = go.AddComponent<ImageExt>();
            com.raycastTarget = false;
            return com;
        }

        public static EmptyGraphic AddEmptyGraphic(this GameObject go)
        {
            EmptyGraphic com = go.GetComponent<EmptyGraphic>();
            if (com != null)
            {
                return com;
            }

            com = go.AddComponent<EmptyGraphic>();
            com.raycastTarget = true;
            return com;
        }

        public static RawImageExt AddRawImage(this GameObject go)
        {
            RawImageExt com = go.GetComponent<RawImageExt>();
            if (com != null)
            {
                return com;
            }

            com = go.AddComponent<RawImageExt>();
            com.raycastTarget = false;
            return com;
        }

        public static ButtonExt AddButton(this GameObject go)
        {
            ButtonExt com = go.GetComponent<ButtonExt>();
            if (com != null)
            {
                return com;
            }

            com = go.AddComponent<ButtonExt>();
            EmptyGraphic emptyGraphic = go.AddEmptyGraphic();
            com.targetGraphic = emptyGraphic;
            return com;
        }

        public static TMPExt AddTMP(this GameObject go)
        {
            TMPExt com = go.GetComponent<TMPExt>();
            if (com != null)
            {
                return com;
            }

            com = go.AddComponent<TMPExt>();
            com.raycastTarget = false;
            return com;
        }

        public static TextExt AddText(this GameObject go)
        {
            TextExt com = go.GetComponent<TextExt>();
            if (com != null)
            {
                return com;
            }

            com = go.AddComponent<TextExt>();
            com.raycastTarget = false;
            com.fontSize = 32;
            com.alignment = TextAnchor.MiddleLeft;
            return com;
        }
    }
}
