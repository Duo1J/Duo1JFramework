using Duo1JFramework;
using Duo1JFramework.Asset;
using Duo1JFramework.ObjectPool;
using Duo1JFramework.UI;
using UnityEditor;
using UnityEngine;

public class InfoWindow : Window
{
    TextExtend buttomInfoText;

    protected override UIConfig CreateUIConfig()
    {
        return new UIConfig("UI/InfoWindow")
            .SetLoadType(EAssetLoadType.Resources)
            .SetLayer(EUILayer.Const);
    }

    protected override void OnDispose()
    {
    }

    protected override void OnInit()
    {
#if UNITY_EDITOR

        buttomInfoText = GetCom<TextExtend>("BottomInfo");

        RegisterUpdate(() =>
        {
            Pool.StringBuilderPool.Using((sb) =>
            {
                sb.Append($"Res:{UnityStats.screenRes}");
                sb.Append(string.Format(" - FrameTime:{0:f7}", UnityStats.frameTime));
                sb.Append(string.Format(" - RenderTime:{0:f7}", UnityStats.renderTime));
                sb.Append(string.Format(" - UnscaledTime:{0:f2}", Time.unscaledTime));
                buttomInfoText.text = sb.ToString();
            });
        });

#endif
    }
}
