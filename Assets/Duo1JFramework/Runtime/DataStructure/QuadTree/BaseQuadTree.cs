using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 四叉树基类
    /// </summary>
    public abstract class BaseQuadTree : IQuadTreeNode, IGizmosDrawer
    {
        /// <summary>
        /// 根节点
        /// </summary>
        protected IQuadTreeNode root;

        /// <summary>
        /// 包围盒
        /// </summary>
        public Bounds Bounds
        {
            get
            {
                if (root == null)
                {
                    Log.ErrorForce("四叉树根节点未初始化");
                    return new Bounds();
                }

                return root.Bounds;
            }
        }

        /// <summary>
        /// 评估检测算法
        /// </summary>
        public virtual Func<IQuadTreeNode, object, bool> EvalLogic
        {
            get => evalLogic;
            set => evalLogic = value;
        }
        protected Func<IQuadTreeNode, object, bool> evalLogic;

        /// <summary>
        /// 最大深度
        /// </summary>
        public int MaxDepth { get; protected set; } = 5;

        /// <summary>
        /// 子节点数量
        /// </summary>
        public const int CHILD_COUNT = 4;

        /// <summary>
        /// 管理对象列表
        /// </summary>
        protected List<IQuadTreeItem> itemList = new List<IQuadTreeItem>();

        /// <summary>
        /// 添加对象
        /// </summary>
        public void AddItem(IQuadTreeItem item)
        {
            root.AddItem(item);
            itemList.Add(item);
        }

        /// <summary>
        /// 移除对象
        /// </summary>
        public bool RemoveItem(IQuadTreeItem item)
        {
            itemList.Remove(item);
            return root.RemoveItem(item);
        }

        /// <summary>
        /// 检测评估
        /// </summary>
        public abstract void Evaluate(object param = null);

        /// <summary>
        /// 绘制Gizmos信息
        /// </summary>
        public abstract void DrawGizmos();

        /// <summary>
        /// 清除
        /// </summary>
        public virtual void Clear()
        {
            root = null;
            itemList = null;
        }
    }
}
