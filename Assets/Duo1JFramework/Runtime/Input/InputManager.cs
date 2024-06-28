using Duo1JFramework.UI;
using System;
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

        private static EInputLimit limit = EInputLimit.All;

        /// <summary>
        /// 设置鼠标是否可见
        /// </summary>
        public static void SetCursorVisible(bool visible)
        {
            Cursor.visible = visible;
            if (visible)
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        /// <summary>
        /// 忽略输入限制
        /// </summary>
        public static void IgnoreLimit(Action action)
        {
            ExecByLimit(EInputLimit.All, action);
        }

        /// <summary>
        /// 使用指定的输入限制执行
        /// </summary>
        public static void ExecByLimit(EInputLimit tarLimit, Action action)
        {
            EInputLimit saveLimit = limit;
            try
            {
                limit = tarLimit;
                action?.Invoke();
            }
            finally
            {
                limit = saveLimit;
            }
        }

        /// <summary>
        /// 水平轴输入
        /// </summary>
        public static float GetAxisH(bool raw = false)
        {
            if (!CheckLimit(EInputLimit.Axis)) return 0;
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
            if (!CheckLimit(EInputLimit.Axis)) return 0;
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
            if (!CheckLimit(EInputLimit.MouseAxis)) return 0;
            return Input.GetAxis("Mouse X");
        }

        /// <summary>
        /// 获取右舵(鼠标)Y轴
        /// </summary>
        public static float GetAxisMY()
        {
            if (!CheckLimit(EInputLimit.MouseAxis)) return 0;
            return Input.GetAxis("Mouse Y");
        }

        /// <summary>
        /// 按键保持
        /// </summary>
        public static bool GetKey(KeyCode key)
        {
            if (!CheckLimit(EInputLimit.Key)) return false;
            return Input.GetKey(key);
        }

        /// <summary>
        /// 按键按下
        /// </summary>
        public static bool GetKeyDown(KeyCode key)
        {
            if (!CheckLimit(EInputLimit.Key)) return false;
            return Input.GetKeyDown(key);
        }

        /// <summary>
        /// 按键抬起
        /// </summary>
        public static bool GetKeyUp(KeyCode key)
        {
            if (!CheckLimit(EInputLimit.Key)) return false;
            return Input.GetKeyUp(key);
        }

        /// <summary>
        /// 鼠标按键保持
        /// </summary>
        public static bool GetMouseBtn(int mouseBtn)
        {
            if (mouseBtn == Def.Input.INPUT_MOUSE_NONE) return false;
            if (!CheckLimit(EInputLimit.Key)) return false;
            return Input.GetMouseButton(mouseBtn);
        }

        /// <summary>
        /// 鼠标按键按下
        /// </summary>
        public static bool GetMouseBtnDown(int mouseBtn)
        {
            if (mouseBtn == Def.Input.INPUT_MOUSE_NONE) return false;
            if (!CheckLimit(EInputLimit.Key)) return false;
            return Input.GetMouseButtonDown(mouseBtn);
        }

        /// <summary>
        /// 鼠标按键抬起
        /// </summary>
        public static bool GetMouseBtnUp(int mouseBtn)
        {
            if (mouseBtn == Def.Input.INPUT_MOUSE_NONE) return false;
            if (!CheckLimit(EInputLimit.Key)) return false;
            return Input.GetMouseButtonUp(mouseBtn);
        }

        /// <summary>
        /// 设置输入是否可用
        /// </summary>
        public static void SetLimit(EInputLimit _limit, bool isEnable)
        {
            if (_limit == EInputLimit.None)
            {
                if (isEnable)
                {
                    limit = EInputLimit.None;
                    Log.Info("输入全部关闭");
                }
                else
                {
                    limit = EInputLimit.All;
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
        public static bool CheckLimit(EInputLimit _limit)
        {
            return (limit & _limit) > 0;
        }
    }
}