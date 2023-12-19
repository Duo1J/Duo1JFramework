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
        return new UIConfig(Path.RES_PATH_UI_PREFIX + "InfoWindow")
            .SetIsResource(true)
            .SetLayer(UILayer.Const);
    }

    protected override void OnDisposeInner()
    {
    }

    protected override void OnInitInner()
    {
        buttomInfoText = GetCom<TextExtend>("BottomInfo");

        RegisterUpdate(() =>
        {
            Pool.StringBuilderPool.Using((item) =>
            {
                item.Value.Append($"Res:{UnityStats.screenRes}");
                item.Value.Append(string.Format(" - FrameTime:{0:f7}", UnityStats.frameTime));
                item.Value.Append(string.Format(" - RenderTime:{0:f7}", UnityStats.renderTime));
                item.Value.Append(string.Format(" - UnscaledTime:{0:f2}", Time.unscaledTime));
                buttomInfoText.text = item.Value.ToString();
            });
        });
    }
}
