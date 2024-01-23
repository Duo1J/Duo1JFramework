using Cinemachine;
using Duo1JFramework;
using Duo1JFramework.Actor;
using Duo1JFramework.Camera3D;
using Duo1JFramework.GamerInput;
using Duo1JFramework.UI;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private CinemachineVirtualCamera vm;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        Game.TriggerSingleton();
        CameraManager.Instance.InitCamera<CMCamera>("Camera/Camera3rdPerson.prefab");

        UIManager.Instance.OpenWindow(new InfoWindow());

        BaseActor mainActor = ActorManager.Instance.CreateActor(
            new ActorData(typeof(CcControlableActor), "TestActor", "Actor/Actor-02.prefab"));
        ActorManager.Instance.SetMainActor(mainActor, true);

        //todo hlj
        vm = CMBrain.Instance.LoadVirtualCamera("Camera/TestCamera.prefab");
    }

    private void Update()
    {
        if (InputManager.GetKeyDown(KeyCode.H))
        {
            vm.Priority = 11;
        }
        else if (InputManager.GetKeyDown(KeyCode.J))
        {
            vm.Priority = 0;
        }
    }
}
