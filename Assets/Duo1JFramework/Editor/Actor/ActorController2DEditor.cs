using UnityEditor;

namespace Duo1JFramework.Actor.Actor2D
{
    [CustomEditor(typeof(BaseActorController2D), true)]
    public class ActorController2DEditor : BaseCustomEditor<BaseActorController2D>
    {
        protected override void OnEnable()
        {
            base.OnEnable();
        }

        protected override void DrawInspector()
        {
        }
    }
}
