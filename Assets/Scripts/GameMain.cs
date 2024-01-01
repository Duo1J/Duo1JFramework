using Duo1JFramework;
using Duo1JFramework.Actor;
using Duo1JFramework.UI;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        UIManager.Instance.OpenWindow(new InfoWindow());

        ActorManager.Instance.CreateActor(
            new ActorData(typeof(ControlableActor), "TestActor", "Actor/Actor-01.prefab"));
    }

    private void Update()
    {
    }
}
