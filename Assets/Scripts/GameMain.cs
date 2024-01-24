using Duo1JFramework;
using Duo1JFramework.Actor;
using Duo1JFramework.Camera3D;
using Duo1JFramework.UI;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private BaseActor mainActor;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Game.TriggerSingleton();
        CameraManager.Instance.InitCamera<CMCamera>("Camera/Camera3rdPerson.prefab");

        UIManager.Instance.OpenWindow(new InfoWindow());

        mainActor = ActorManager.Instance.CreateActor(
            new ActorData(typeof(CcControlableActor), "TestActor", "Actor/Actor-02.prefab"));
        ActorManager.Instance.SetMainActor(mainActor, true);
    }

    private void Update()
    {
    }
}
