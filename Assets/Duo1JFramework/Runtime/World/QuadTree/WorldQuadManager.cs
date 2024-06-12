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
        public WorldQuadContainer Container
        {
            set
            {
                if (container != null)
                {
                    container.DestroyImmediate();
                }
                container = value;
                container.CreateTree();
            }
        }

        private WorldQuadContainer container;

        public Bounds Bounds => container ? container.Bounds : new Bounds();

        /// <summary>
        /// 检测参数获取委托
        /// </summary>
        private Func<object> paramGetFunc;

        /// <summary>
        /// 添加对象
        /// </summary>
        public void AddItem(IQuadTreeItem item)
        {
            Assert.NotNull(container, "四叉树容器未初始化");
            container.AddItem(item);
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        public bool RemoveItem(IQuadTreeItem item)
        {
            Assert.NotNull(container, "四叉树容器未初始化");
            return container.RemoveItem(item);
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
        /// 设置评估检测算法
        /// </summary>
        /// <see cref="QuadTreeEvalLogic"/>
        /// <param name="evalLogic">QuadTreeEvalLogic</param>
        /// <param name="paramGetFunc">检测参数获取委托</param>
        public void SetEvalLogic(Func<IQuadTreeNode, object, bool> evalLogic, Func<object> paramGetFunc = null)
        {
            container.SetEvalLogic(evalLogic);
            this.paramGetFunc = paramGetFunc;
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
        }

        protected override void OnInit()
        {
            Register.RegisterPreUpdate(OnPreUpdate);
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
        }
#endif
    }
}