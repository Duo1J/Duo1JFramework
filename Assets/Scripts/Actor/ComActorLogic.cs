using Duo1JFramework.Actor;
using Duo1JFramework.FSM;
using Duo1JFramework.GamerInput;

public class ComActorLogic : CcControlableActor
{
    protected override void OnCreated()
    {
        base.OnCreated();

        Con.AddFSMNode(
            StateNode.Create(
                "Box1",
                () =>
                {
                    Con.AniCrossFade("Boxing01");
                }
            ).TimeToState(1.2f, "Move"));

        Con.AddFSMNode(
            StateNode.Create(
                "Box2",
                () =>
                {
                    Con.AniCrossFade("Boxing02");
                }
            ).TimeToState(1.2f, "Move"));

        //todo hlj
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        if (InputManager.GetMouseBtn(0))
        {
            Con.SwitchState("Box1");
        }
        else if (InputManager.GetMouseBtn(1))
        {
            Con.SwitchState("Box2");
        }
    }
}
