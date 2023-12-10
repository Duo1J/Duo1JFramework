using Duo1JFramework.UI;
using UnityEngine;

public class LoginWindow : Window
{
    protected override UIConfig CreateUIConfig()
    {
        return new UIConfig("UI/LoginWindow.prefab");
    }
}
