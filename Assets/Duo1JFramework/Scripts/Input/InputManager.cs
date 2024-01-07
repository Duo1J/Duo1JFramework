using Duo1JFramework.UI;
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
        /// 获取椭圆映射后的Raw-HV轴
        /// </summary>
        public static void GetCircleMapAxisRaw(out float h, out float v)
        {
            h = GetAxisH(true);
            v = GetAxisV(true);
            MathUtil.CircleMapping(ref h, ref v);
        }

        /// <summary>
        /// 获取椭圆映射后的HV轴
        /// </summary>
        public static void GetCircleMapAxis(out float h, out float v)
        {
            h = GetAxisH(false);
            v = GetAxisV(false);
            MathUtil.CircleMapping(ref h, ref v);
        }

        /// <summary>
        /// 获取右舵(鼠标)X轴
        /// </summary>
        public static float GetAxisMX()
        {
            return Input.GetAxis("Mouse X");
        }

        /// <summary>
        /// 获取右舵(鼠标)Y轴
        /// </summary>
        public static float GetAxisMY()
        {
            return Input.GetAxis("Mouse Y");
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