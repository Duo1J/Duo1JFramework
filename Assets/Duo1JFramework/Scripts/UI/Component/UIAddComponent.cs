using Unity.VisualScripting;
using UnityEngine;

namespace Duo1JFramework.UI
{
    public static class UIAddComponent
    {
        public static ImageExtend AddImage(this GameObject go)
        {
            ImageExtend com = go.GetComponent<ImageExtend>();
            if (com != null) return com;
            com = go.AddComponent<ImageExtend>();
            com.raycastTarget = false;
            return com;
        }

        public static EmptyGraphic AddEmptyGraphic(this GameObject go)
        {
            EmptyGraphic com = go.GetComponent<EmptyGraphic>();
            if (com != null) return com;
            com = go.AddComponent<EmptyGraphic>();
            com.raycastTarget = true;
            return com;
        }

        public static RawImageExtend AddRawImage(this GameObject go)
        {
            RawImageExtend com = go.GetComponent<RawImageExtend>();
            if (com != null) return com;
            com = go.AddComponent<RawImageExtend>();
            com.raycastTarget = false;
            return com;
        }

        public static ButtonExtend AddButton(this GameObject go)
        {
            ButtonExtend com = go.GetComponent<ButtonExtend>();
            if (com != null) return com;
            com = go.AddComponent<ButtonExtend>();
            EmptyGraphic emptyGraphic = go.AddEmptyGraphic();
            com.targetGraphic = emptyGraphic;
            return com;
        }

        public static TMPExtend AddTMP(this GameObject go)
        {
            TMPExtend com = go.GetComponent<TMPExtend>();
            if (com != null) return com;
            com = go.AddComponent<TMPExtend>();
            com.raycastTarget = false;
            return com;
        }

        public static TextExtend AddText(this GameObject go)
        {
            TextExtend com = go.GetComponent<TextExtend>();
            if (com != null) return com;
            com = go.AddComponent<TextExtend>();
            com.raycastTarget = false;
            com.fontSize = 32;
            com.alignment = TextAnchor.MiddleLeft;
            return com;
        }
    }
}