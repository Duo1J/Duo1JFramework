using Duo1JFramework.DataStructure;
using System;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界场景四叉树控制器, 用于管理受四叉树控制物体, 可作为四叉树容器的子树
    /// </summary>
    /// <see cref="WorldQuadContainer"/>
    [RequireComponent(typeof(GizmosBounds))]
    public class WorldQuadController : WorldQuadItem, IQuadTreeNode
    {
        private QuadTree tree;

        /// <summary>
        /// 添加对象
        /// </summary>
        public void AddItem(IQuadTreeItem item)
        {
            Assert.NotNull(tree, "四叉树未初始化");
            tree.AddItem(item);
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        public bool RemoveItem(IQuadTreeItem item)
        {
            Assert.NotNull(tree, "四叉树未初始化");
            return tree.RemoveItem(item);
        }

        /// <summary>
        /// 检测评估
        /// </summary>
        public void Evaluate(object param)
        {
            Assert.NotNull(tree, "四叉树未初始化");

            if (!QuadActive)
            {
                tree.UnActive();
                return;
            }

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
        }

        protected override void Start()
        {
            base.Start();
            CreateTree();
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