using Duo1JFramework;
using Duo1JFramework.UI;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private void Awake()
    {
    }

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            UIManager.Instance.OpenWindow(new LoginWindow());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            UIManager.Instance.CloseWindow(UIManager.Instance.GetWindow(typeof(LoginWindow)));
        }
    }
}
