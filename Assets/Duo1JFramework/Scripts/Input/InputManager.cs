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
        public static float GetAxisH(bool raw = false)
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
        public static float GetAxisV(bool raw = false)
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
        public static bool GetKey(KeyCode key)
        {
            return Input.GetKey(key);
        }

        public static bool GetKeyDown(KeyCode key)
        {
            return Input.GetKeyDown(key);
        }

        public static bool GetKeyUp(KeyCode key)
        {
            return Input.GetKeyUp(key);
        }
    }
}