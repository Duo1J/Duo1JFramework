using Duo1JFramework.DataStructure;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界场景四叉树容器, 用于管理四叉树控制器
    /// </summary>
    /// <see cref="WorldQuadController"/>
    [RequireComponent(typeof(GizmosBounds))]
    public class WorldQuadContainer : WorldQuadItem, IQuadTreeNode
    {
        private QuadTree tree;

        private HashSet<WorldQuadController> controllerSet;

        /// <summary>
        /// 添加对象 
        /// </summary>
        public void AddItem(IQuadTreeItem item)
        {
            if (item is WorldQuadController controller)
            {
                controllerSet.Add(controller);
                return;
            }

            bool add = false;
            foreach (WorldQuadController con in controllerSet)
            {
                if (QuadTreeEvalLogic.CheckByBounds(con, item))
                {
                    add = true;
                    con.AddItem(item);
                }
            }

            if (!add)
            {
                Assert.NotNull(tree, "四叉树未初始化");
                tree.AddItem(item);
            }
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        public bool RemoveItem(IQuadTreeItem item)
        {
            if (item is WorldQuadController controller)
            {
                return controllerSet.Remove(controller);
            }

            bool remove = false;
            foreach (WorldQuadController con in controllerSet)
            {
                if (QuadTreeEvalLogic.CheckByBounds(con, item))
                {
                    bool flag = con.RemoveItem(item);
                    if (remove == false)
                    {
                        remove = flag;
                    }
                }
            }

            if (!remove)
            {
                Assert.NotNull(tree, "四叉树未初始化");
                return tree.RemoveItem(item);
            }

            return true;
        }

        /// <summary>
        /// 检测评估
        /// </summary>
        public void Evaluate(object param)
        {
            foreach (WorldQuadController con in controllerSet)
            {
                con.SetQuadState(tree.EvalLogic(con, param));
                con.Evaluate(param);
            }

            Assert.NotNull(tree, "四叉树未初始化");
            tree.Evaluate(param);
        }

        /// <summary>
        /// 重建四叉树
        /// </summary>
        public void RebuildTree(Bounds bounds, int maxDepth = QuadTree.DEFAULT_DEPTH)
        {
            GizmosBounds.Bounds = bounds;
            if (tree == null)
            {
                CreateTree(maxDepth);
            }
            else
            {
                tree.Rebuild(bounds, maxDepth);
            }
        }

        /// <summary>
        /// 创建四叉树
        /// </summary>
        public void CreateTree(int maxDepth = QuadTree.DEFAULT_DEPTH)
        {
            tree = QuadTree.Create(GizmosBounds.Bounds, maxDepth);
        }

        /// <summary>
        /// 设置评估检测算法
        /// </summary>
        /// <see cref="QuadTreeEvalLogic"/>
        /// <param name="evalLogic">QuadTreeEvalLogic</param>
        public void SetEvalLogic(Func<IQuadTreeNode, object, bool> evalLogic)
        {
            Assert.NotNull(tree, "四叉树未初始化");

            tree.EvalLogic = evalLogic;
            foreach (WorldQuadController con in controllerSet)
            {
                con.SetEvalLogic(evalLogic);
            }
        }

        private void Awake()
        {
            controllerSet = new HashSet<WorldQuadController>();

            WorldQuadManager.Instance.Container = this;
        }

        protected override void Start()
        {
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }

#if UNITY_EDITOR

        public bool drawGizmosUnSelected = false;

        private void OnDrawGizmos()
        {
            if (drawGizmosUnSelected)
            {
                tree?.DrawGizmos();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (!drawGizmosUnSelected)
            {
                tree?.DrawGizmos();
            }
        }

#endif
    }
}