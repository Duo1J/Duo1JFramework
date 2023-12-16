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
        UIManager.Instance.OpenWindow(new InfoWindow());
    }

    private void Update()
    {
    }
}
