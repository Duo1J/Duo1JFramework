using Duo1JFramework;
using Duo1JFramework.Actor;
using Duo1JFramework.Asset;
using Duo1JFramework.Camera3D;
using Duo1JFramework.GamerInput;
using Duo1JFramework.Timeline;
using Duo1JFramework.TimerUpdate;
using Duo1JFramework.UI;
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
        CameraManager.Instance.InitCamera<CMCamera>("Camera/Camera3rdPerson.prefab");
        UIManager.Instance.OpenWindow(new InfoWindow());

        mainActor = ActorManager.Instance.CreateActor(
            new ActorData(typeof(CcControlableActor), "TestActor", "Actor/Actor-02.prefab"));
        ActorManager.Instance.SetMainActor(mainActor, true);
    }

    private void Update()
    {
        if (InputManager.GetKeyDown(KeyCode.E))
        {
            TimelineManager.Instance.LoadTimeline("Timeline/Timeline-01.prefab", (td) =>
            {
                InputManager.SetLimit(InputLimit.All, false);
                td.SetDestroyCallback((td) =>
                {
                    InputManager.SetLimit(InputLimit.All, true);
                });
                td.SyncTransform(ActorManager.Instance.MainActor);
                td.SetGenericBinding("CinemachineBrain", CMBrain.Instance.Brain);
                td.DestroyOnStop();
                td.Play();
            });
        }
    }
}
