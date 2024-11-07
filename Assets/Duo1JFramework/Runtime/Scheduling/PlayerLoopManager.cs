using System;
using UnityEngine.LowLevel;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Duo1JFramework.Scheduling
{
    /// <summary>
    /// 用户循环管理器
    /// </summary>
    public class PlayerLoopManager : MonoSingleton<PlayerLoopManager>
    {
        /// <summary>
        /// 添加用户循环
        /// </summary>
        /// <param name="type">循环类型 `typeof(UnityEngine.PlayerLoop.XXX)`</param>
        public void AddPlayerLoop(Type type, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            PlayerLoopSystem playerLoopSystem = new PlayerLoopSystem
            {
                type = type,
                updateDelegate = updateFunction
            };

            PlayerLoopSystem curPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();

            for (int i = 0; i < curPlayerLoop.subSystemList.Length; i++)
            {
                if (curPlayerLoop.subSystemList[i].type == type)
                {
                    PlayerLoopSystem[] oldSubSystemList = curPlayerLoop.subSystemList[i].subSystemList;
                    PlayerLoopSystem[] newSubSystemList = new PlayerLoopSystem[oldSubSystemList.Length + 1];

                    Array.Copy(oldSubSystemList, newSubSystemList, oldSubSystemList.Length);

                    newSubSystemList[newSubSystemList.Length - 1] = playerLoopSystem;
                    curPlayerLoop.subSystemList[i].subSystemList = newSubSystemList;

                    break;
                }
            }

            PlayerLoop.SetPlayerLoop(curPlayerLoop);
        }

#if UNITY_EDITOR

        /// <summary>
        /// 运行模式状态改变回调
        /// </summary>
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.ExitingPlayMode)
            {
                PlayerLoop.SetPlayerLoop(PlayerLoop.GetDefaultPlayerLoop());
            }
        }

#endif

        protected override void OnInit()
        {
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#endif
        }

        protected override void OnDispose()
        {
        }
    }
}