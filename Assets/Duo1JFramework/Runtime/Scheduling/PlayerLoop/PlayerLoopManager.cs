using System;
using System.Collections.Generic;
using UnityEngine.LowLevel;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Duo1JFramework.Scheduling
{
    /// <summary>
    /// 用户循环管理器
    /// </summary>
    /// <see cref="UpdateManager"/>
    public class PlayerLoopManager : MonoSingleton<PlayerLoopManager>
    {
        /// <summary>
        /// 用户循环体列表
        /// </summary>
        private List<Loop> loopList;

        /// <summary>
        /// 已注入的循环函数集合
        /// </summary>
        private HashSet<PlayerLoopSystem.UpdateFunction> injectedFunctionSet;

        /// <summary>
        /// 注入前的原始PlayerLoop
        /// </summary>
        private PlayerLoopSystem originPlayerLoop;

        /// <summary>
        /// 是否已记录原始PlayerLoop
        /// </summary>
        private bool hasOriginPlayerLoop;

        /// <summary>
        /// 添加用户循环
        /// </summary>
        /// <param name="type">循环类型 `typeof(UnityEngine.PlayerLoop.XXX)`</param>
        public Loop AddPlayerLoop(Type type, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            Assert.NotNullArg(type, "type");
            Assert.NotNullArg(updateFunction, "updateFunction");

            if (injectedFunctionSet == null)
            {
                injectedFunctionSet = new HashSet<PlayerLoopSystem.UpdateFunction>();
            }

            if (injectedFunctionSet.Contains(updateFunction))
            {
                Log.Warn($"PlayerLoop已注入 `{updateFunction.Method.Name}`，忽略重复注入");
                return null;
            }

            if (!hasOriginPlayerLoop)
            {
                originPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
                hasOriginPlayerLoop = true;
            }

            Loop loop = new Loop(type, updateFunction);

            PlayerLoopSystem playerLoopSystem = new PlayerLoopSystem
            {
                type = type,
                updateDelegate = loop.InjectedFunction
            };

            PlayerLoopSystem curPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            bool added = false;

            for (int i = 0; i < curPlayerLoop.subSystemList.Length; i++)
            {
                if (curPlayerLoop.subSystemList[i].type == type)
                {
                    PlayerLoopSystem[] oldSubSystemList = curPlayerLoop.subSystemList[i].subSystemList ?? new PlayerLoopSystem[0];
                    PlayerLoopSystem[] newSubSystemList = new PlayerLoopSystem[oldSubSystemList.Length + 1];

                    Array.Copy(oldSubSystemList, newSubSystemList, oldSubSystemList.Length);

                    newSubSystemList[newSubSystemList.Length - 1] = playerLoopSystem;
                    curPlayerLoop.subSystemList[i].subSystemList = newSubSystemList;
                    added = true;

                    break;
                }
            }

            if (!added)
            {
                Log.ErrorForce($"未找到PlayerLoop节点: `{type.FullName}`");
                loop.Dispose();
                return null;
            }

            PlayerLoop.SetPlayerLoop(curPlayerLoop);
            injectedFunctionSet.Add(updateFunction);

            if (loopList == null)
            {
                loopList = new List<Loop>();
            }

            loopList.Add(loop);

            return loop;
        }

        /// <summary>
        /// 移除用户循环
        /// </summary>
        public bool RemovePlayerLoop(Loop loop)
        {
            if (loop == null || loop.Disposed)
            {
                return false;
            }

            return RemovePlayerLoop(loop.Type, loop.UpdateFunction, loop);
        }

        /// <summary>
        /// 移除用户循环
        /// </summary>
        public bool RemovePlayerLoop(Type type, PlayerLoopSystem.UpdateFunction updateFunction)
        {
            Assert.NotNullArg(type, "type");
            Assert.NotNullArg(updateFunction, "updateFunction");

            return RemovePlayerLoop(type, updateFunction, null);
        }

        private bool RemovePlayerLoop(Type type, PlayerLoopSystem.UpdateFunction updateFunction, Loop loop)
        {
            Assert.NotNullArg(type, "type");
            Assert.NotNullArg(updateFunction, "updateFunction");

            if (loop == null && loopList != null)
            {
                loop = loopList.Find(item => item != null && item.Type == type && item.UpdateFunction == updateFunction);
            }

            PlayerLoopSystem.UpdateFunction injectedFunction = loop == null ? updateFunction : loop.InjectedFunction;
            PlayerLoopSystem curPlayerLoop = PlayerLoop.GetCurrentPlayerLoop();
            bool removed = false;

            for (int i = 0; i < curPlayerLoop.subSystemList.Length; i++)
            {
                if (curPlayerLoop.subSystemList[i].type != type)
                {
                    continue;
                }

                PlayerLoopSystem[] oldSubSystemList = curPlayerLoop.subSystemList[i].subSystemList;
                if (oldSubSystemList == null || oldSubSystemList.Length == 0)
                {
                    break;
                }

                int removeIndex = -1;
                for (int j = 0; j < oldSubSystemList.Length; j++)
                {
                    if (oldSubSystemList[j].updateDelegate == injectedFunction)
                    {
                        removeIndex = j;
                        break;
                    }
                }

                if (removeIndex < 0)
                {
                    break;
                }

                PlayerLoopSystem[] newSubSystemList = new PlayerLoopSystem[oldSubSystemList.Length - 1];
                if (removeIndex > 0)
                {
                    Array.Copy(oldSubSystemList, 0, newSubSystemList, 0, removeIndex);
                }

                if (removeIndex < oldSubSystemList.Length - 1)
                {
                    Array.Copy(oldSubSystemList, removeIndex + 1, newSubSystemList, removeIndex, oldSubSystemList.Length - removeIndex - 1);
                }

                curPlayerLoop.subSystemList[i].subSystemList = newSubSystemList;
                removed = true;
                break;
            }

            if (!removed)
            {
                return false;
            }

            PlayerLoop.SetPlayerLoop(curPlayerLoop);
            injectedFunctionSet?.Remove(updateFunction);

            if (loop != null)
            {
                loopList?.Remove(loop);
                loop.Dispose();
            }

            return true;
        }

        private void DisposeAllLoop()
        {
            if (loopList != null)
            {
                foreach (Loop loop in loopList)
                {
                    loop?.Dispose();
                }

                loopList.Clear();
                loopList = null;
            }

            injectedFunctionSet?.Clear();

            if (hasOriginPlayerLoop)
            {
                PlayerLoop.SetPlayerLoop(originPlayerLoop);
                hasOriginPlayerLoop = false;
            }
        }

#if UNITY_EDITOR

        /// <summary>
        /// 运行模式状态改变回调
        /// </summary>
        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingEditMode || state == PlayModeStateChange.ExitingPlayMode)
            {
                DisposeAllLoop();
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
#if UNITY_EDITOR
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
#endif
            DisposeAllLoop();
        }
    }
}