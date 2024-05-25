using Duo1JFramework;
using Duo1JFramework.Actor;
using Duo1JFramework.Asset;
using Duo1JFramework.AudioAPI;
using Duo1JFramework.CameraAPI;
using Duo1JFramework.GamerInput;
using Duo1JFramework.ObjectPool;
using Duo1JFramework.PhysicsAPI;
using Duo1JFramework.RX;
using Duo1JFramework.TimelineAPI;
using Duo1JFramework.TimerUpdate;
using Duo1JFramework.UI;
using Duo1JFramework.World;
using System.Text;
using UnityEngine;

public class GameMain : BaseGameMain
{
    private BaseActor mainActor;

    protected override void Awake()
    {
        base.Awake();

#if !UNITY_EDITOR
        InputManager.SetCursorVisible(false);
#endif
    }

    private void Start()
    {
        UIManager.Instance.OpenWindow<InfoWindow>();
        WorldManager.Instance.LoadWorld(new WorldData("World01", "World/World-01/World-01.prefab"), (controller) =>
        {
            CameraManager.Instance.InitCamera<CMCamera>("Camera/Camera3rdPerson.prefab");

            mainActor = ActorManager.Instance.CreateActor(new ActorData(typeof(ComActorLogic), "TestActor", "Actor/Actor-02.prefab"));
            ActorManager.Instance.SetMainActor(mainActor, true);

            GameObject.Find("QuadItemList").transform.ChildForeach((childGo) =>
            {
                WorldQuadManager.Instance.AddItem(childGo.GetOrAddComponent<WorldQuadItem>());
            });
        });
    }

    private void Update()
    {
        if (InputManager.GetKeyDown(KeyCode.E))
        {
            TimelineManager.Instance.LoadTimeline("Timeline/Timeline-01.prefab", (td) =>
            {
                InputManager.SetLimit(EInputLimit.All, false);
                td.SetDestroyCallback((td) =>
                {
                    InputManager.SetLimit(EInputLimit.All, true);
                });
                td.SyncTransform(ActorManager.Instance.MainActor);
                td.SetGenericBinding("CinemachineBrain", CMBrain.Instance.Brain);
                td.DestroyOnStop();
                td.Play();
            });
        }

#if !UNITY_EDITOR
        if (InputManager.GetKey(KeyCode.LeftControl))
        {
            InputManager.SetCursorVisible(true);
        }
        else
        {
            InputManager.SetCursorVisible(false);
        }
#endif
    }
}
