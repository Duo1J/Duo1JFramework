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
        CameraManager.Instance.InitCamera<CMCamera>("Camera/Camera3rdPerson.prefab");
        UIManager.Instance.OpenWindow<InfoWindow>();

        mainActor = ActorManager.Instance.CreateActor(new ActorData(typeof(ComActorLogic), "TestActor", "Actor/Actor-02.prefab"));
        ActorManager.Instance.SetMainActor(mainActor, true);

        AudioManager.Instance.PlayOneShot(new AudioData("Audio/Lena Raine - Quiet and Falling.mp3"));
    }

    private void Update()
    {
        if (InputManager.GetKeyDown(KeyCode.E))
        {
            TimelineManager.Instance.LoadTimeline("Timeline/Timeline-01.prefab", (td) =>
            {
                InputManager.SetLimit(eInputLimit.All, false);
                td.SetDestroyCallback((td) =>
                {
                    InputManager.SetLimit(eInputLimit.All, true);
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
