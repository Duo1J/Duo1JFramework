using Duo1JFramework.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoWindow : Window
{
    protected override UIConfig CreateUIConfig()
    {
        return new UIConfig("UI/InfoWindow.prefab");
    }
}
