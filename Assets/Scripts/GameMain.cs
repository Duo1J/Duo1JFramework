using Duo1JFramework;
using Duo1JFramework.Actor;
using Duo1JFramework.Camera3D;
using Duo1JFramework.GamerInput;
using Duo1JFramework.Timeline;
using Duo1JFramework.UI;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    private BaseActor mainActor;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Framework.Init();
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
                //todo hlj virtual camera pos
                //rotate td.GO
                td.SetGenericBinding("MainActorRun", ActorManager.Instance.MainActor.GetAnimator());
                td.SetGenericBinding("CinemachineBrain", CMBrain.Instance.Brain);
                td.Play();
            });
        }
    }
}
