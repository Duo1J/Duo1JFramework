using UnityEngine;

namespace Duo1JFramework.GamerInput
{
    public static class InputManager
    {
        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";

        /// <summary>
        /// 水平轴输入
        /// </summary>
        public static float HAxis(bool raw = false)
        {
            if (raw)
            {
                return Input.GetAxisRaw(HORIZONTAL);
            }
            else
            {
                return Input.GetAxis(HORIZONTAL);
            }
        }

        /// <summary>
        /// 垂直轴输入
        /// </summary>
        public static float VAxis(bool raw = false)
        {
            if (raw)
            {
                return Input.GetAxisRaw(VERTICAL);
            }
            else
            {
                return Input.GetAxis(VERTICAL);
            }
        }

        /// <summary>
        /// 按键保持
        /// </summary>
        public static bool Key(KeyCode key)
        {
            return Input.GetKey(key);
        }

        public static bool KeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public static bool KeyUp(KeyCode key)
        {
            return Input.GetKeyUp(key);
        }
    }
}