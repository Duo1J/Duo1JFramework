using Duo1JFramework.DataStructure;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界四叉树管理器
    /// </summary>
    public class WorldQuadManager : MonoSingleton<WorldQuadManager>, IQuadTreeNode
    {
        public WorldQuadContainer Container
        {
            set
            {
                if (value == null)
                {
                    container = null;
                    return;
                }

                if (container != null && container != value)
                {
                    Log.Warn("世界四叉树容器已存在，将替换为新的容器");
                    container.DestroySmart();
                }

                container = value;
                container.CreateTree();

                if (evalLogic != null)
                {
                    container.SetEvalLogic(evalLogic);
                }

                FlushPendingItems();
            }
        }

        private WorldQuadContainer container;

        public Bounds Bounds => container ? container.Bounds : new Bounds();

        /// <summary>
        /// 检测参数获取委托
        /// </summary>
        private Func<object> paramGetFunc;

        /// <summary>
        /// 检测算法
        /// </summary>
        private Func<IQuadTreeNode, object, bool> evalLogic;

        /// <summary>
        /// 等待加入四叉树的对象
        /// </summary>
        private HashSet<IQuadTreeItem> pendingItemSet;

        /// <summary>
        /// 添加对象
        /// </summary>
        public void AddItem(IQuadTreeItem item)
        {
            Assert.NotNullArg(item, "item");

            if (container == null)
            {
                pendingItemSet.Add(item);
                return;
            }

            container.AddItem(item);
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        public bool RemoveItem(IQuadTreeItem item)
        {
            Assert.NotNullArg(item, "item");

            if (pendingItemSet.Remove(item))
            {
                return true;
            }

            if (container == null)
            {
                return false;
            }

            return container.RemoveItem(item);
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        public void UpdateItem(IQuadTreeItem item)
        {
            Assert.NotNullArg(item, "item");

            if (container == null)
            {
                pendingItemSet.Add(item);
                return;
            }

            container.UpdateItem(item);
        }

        /// <summary>
        /// 检测评估
        /// </summary>
        public void Evaluate(object param)
        {
            Assert.NotNull(container, "四叉树容器树未初始化");
            container.Evaluate(param);
        }

        /// <summary>
        /// 清理容器引用
        /// </summary>
        public void ClearContainer(WorldQuadContainer container)
        {
            if (this.container == container)
            {
                this.container = null;
            }
        }

        /// <summary>
        /// 设置评估检测算法
        /// </summary>
        /// <see cref="QuadTreeEvalLogic"/>
        /// <param name="evalLogic">QuadTreeEvalLogic</param>
        /// <param name="paramGetFunc">检测参数获取委托</param>
        public void SetEvalLogic(Func<IQuadTreeNode, object, bool> evalLogic, Func<object> paramGetFunc = null)
        {
            this.evalLogic = evalLogic;
            this.paramGetFunc = paramGetFunc;

            if (container != null)
            {
                container.SetEvalLogic(evalLogic);
            }
        }

        /// <summary>
        /// 设置评估检测策略
        /// </summary>
        public void SetEvalStrategy(IQuadTreeEvalStrategy evalStrategy, Func<object> paramGetFunc = null)
        {
            SetEvalLogic(evalStrategy == null ? null : evalStrategy.Evaluate, paramGetFunc);
        }

        private void FlushPendingItems()
        {
            foreach (IQuadTreeItem item in pendingItemSet)
            {
                container.AddItem(item);
            }
            pendingItemSet.Clear();
        }

        private void OnPreUpdate()
        {
            if (container != null)
            {
                container.Evaluate(paramGetFunc?.Invoke());
            }
        }

        protected override void OnDispose()
        {
            pendingItemSet.Clear();
            pendingItemSet = null;
            container = null;
            evalLogic = null;
            paramGetFunc = null;
        }

        protected override void OnInit()
        {
            pendingItemSet = new HashSet<IQuadTreeItem>();
            Reg.RegisterPreUpdate(OnPreUpdate);
        }

#if UNITY_EDITOR
        public void DrawEditorInfo()
        {
            GUILayout.Label($"容器: {(container == null ? "无" : container.name)}");
            GUILayout.Label($"等待注册对象数量: {pendingItemSet.Count}");
            GUILayout.Label($"评估算法: {(evalLogic == null ? "默认" : evalLogic.Method.Name)}");
        }

        private void OnDrawGizmos()
        {
        }
#endif
    }
}
