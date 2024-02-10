using UnityEditor;

namespace Duo1JFramework
{
    public abstract class EditorWindowBase : EditorWindow
    {
        public float X => position.x;
        public float Y => position.y;
        public float Width => position.width;
        public float Height => position.height;
    }
}
