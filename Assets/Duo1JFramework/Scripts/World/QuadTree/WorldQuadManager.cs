using Duo1JFramework.DataStructure;
using System;
using UnityEngine;

namespace Duo1JFramework.World
{
    /// <summary>
    /// 世界四叉树管理器
    /// </summary>
    public class WorldQuadManager : MonoSingleton<WorldQuadManager>, IQuadTreeNode
    {
        private QuadTree tree;
        private Func<object> paramGetFunc;

        public bool gizmos = true;

        /// <summary>
        /// 添加对象
        /// </summary>
        public void AddItem(QuadTreeItem item)
        {
            Assert.NotNull(tree, "四叉树未初始化");
            tree.AddItem(item);
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        public bool RemoveItem(QuadTreeItem item)
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
            tree.Evaluate(param);
        }

        /// <summary>
        /// 设置评估检测算法
        /// </summary>
        /// <see cref="QuadTreeEvalLogic"/>
        /// <param name="evalLogic">QuadTreeEvalLogic</param>
        /// <param name="paramGetFunc">检测参数获取委托</param>
        public void SetEvalLogic(Func<QuadTreeNode, object, bool> evalLogic, Func<object> paramGetFunc = null)
        {
            Assert.NotNull(tree, "四叉树未初始化");
            tree.EvalLogic = evalLogic;
            this.paramGetFunc = paramGetFunc;
        }

        /// <summary>
        /// 重建四叉树
        /// </summary>
        public void RebuildTree(Bounds bounds, int maxDepth = QuadTree.DEFAULT_DEPTH)
        {
            if (tree == null)
            {
                CreateTree(bounds, maxDepth);
            }
            else
            {
                tree.Rebuild(bounds, maxDepth);
            }
        }

        /// <summary>
        /// 创建四叉树
        /// </summary>
        public void CreateTree(Bounds bounds, int maxDepth = QuadTree.DEFAULT_DEPTH)
        {
            tree = QuadTree.Create(bounds, maxDepth);
        }

        private void OnUpdate()
        {
            if (tree == null)
            {
                return;
            }

            tree.Evaluate(paramGetFunc?.Invoke());
        }

        private void OnDrawGizmos()
        {
            if (gizmos && tree != null)
            {
                tree.DrawGizmos();
            }
        }

        protected override void OnDispose()
        {
            tree.Clear();
            tree = null;
        }

        protected override void OnInit()
        {
            Register.RegisterUpdate(OnUpdate);
        }
    }
}