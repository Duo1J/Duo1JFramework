using Duo1JFramework.UI;
using UnityEngine;

namespace Duo1JFramework.GamerInput
{
    /// <summary>
    /// 输入管理器
    /// </summary>
    public static class InputManager
    {
        public const string HORIZONTAL = "Horizontal";
        public const string VERTICAL = "Vertical";

        private static InputLimit limit = InputLimit.All;

        /// <summary>
        /// 水平轴输入
        /// </summary>
        public static float GetAxisH(bool raw = false)
        {
            if (!CheckLimit(InputLimit.Axis)) return 0;
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
            if (!CheckLimit(InputLimit.Axis)) return 0;
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
            if (!CheckLimit(InputLimit.MouseAxis)) return 0;
            return Input.GetAxis("Mouse X");
        }

        /// <summary>
        /// 获取右舵(鼠标)Y轴
        /// </summary>
        public static float GetAxisMY()
        {
            if (!CheckLimit(InputLimit.MouseAxis)) return 0;
            return Input.GetAxis("Mouse Y");
        }

        /// <summary>
        /// 按键保持
        /// </summary>
        public static bool GetKey(KeyCode key)
        {
            if (!CheckLimit(InputLimit.Key)) return false;
            return Input.GetKey(key);
        }

        /// <summary>
        /// 按键按下
        /// </summary>
        public static bool GetKeyDown(KeyCode key)
        {
            if (!CheckLimit(InputLimit.Key)) return false;
            return Input.GetKeyDown(key);
        }

        /// <summary>
        /// 按键抬起
        /// </summary>
        public static bool GetKeyUp(KeyCode key)
        {
            if (!CheckLimit(InputLimit.Key)) return false;
            return Input.GetKeyUp(key);
        }

        /// <summary>
        /// 设置输入是否可用
        /// </summary>
        public static void SetLimit(InputLimit _limit, bool isEnable)
        {
            if (_limit == InputLimit.None)
            {
                if (isEnable)
                {
                    limit = InputLimit.None;
                    Log.Info("输入全部关闭");
                }
                else
                {
                    limit = InputLimit.All;
                    Log.Info("输入全部开启");
                }
                return;
            }

            if (isEnable)
            {
                limit |= _limit;
                Log.Info($"输入开启{_limit}");
            }
            else
            {
                limit &= ~_limit;
                Log.Info($"输入关闭{_limit}");
            }
        }

        /// <summary>
        /// 检测输入是否可用
        /// </summary>
        public static bool CheckLimit(InputLimit _limit)
        {
            return (limit & _limit) > 0;
        }
    }
}