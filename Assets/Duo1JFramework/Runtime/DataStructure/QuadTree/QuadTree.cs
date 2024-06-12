using Duo1JFramework.TimerUpdate;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.DataStructure
{
    /// <summary>
    /// 四叉树
    /// </summary>
    public class QuadTree : IQuadTreeNode, IGizmosDrawer
    {
        /// <summary>
        /// 根节点
        /// </summary>
        private QuadTreeNode root;

        /// <summary>
        /// 评估检测算法
        /// </summary>
        public Func<IQuadTreeNode, object, bool> EvalLogic
        {
            get
            {
                if (evalLogic == null)
                {
                    evalLogic = QuadTreeEvalLogic.EvalByConeOfVisionIgnoreY;
                }
                return evalLogic;
            }
            set => evalLogic = value;
        }
        private Func<IQuadTreeNode, object, bool> evalLogic;

        /// <summary>
        /// 最大深度
        /// </summary>
        public int MaxDepth { get; private set; } = 5;

        /// <summary>
        /// 通过包围盒高度控制
        /// </summary>
        public bool EnableHeight { get; private set; } = false;

        public Bounds Bounds => root.Bounds;

        /// <summary>
        /// 子节点数量
        /// </summary>
        public const int CHILD_COUNT = 4;

        /// <summary>
        /// 默认树深度
        /// </summary>
        public const int DEFAULT_DEPTH = 4;

        /// <summary>
        /// 管理对象列表
        /// </summary>
        private List<IQuadTreeItem> itemList;

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
        public void Evaluate(object param = null)
        {
            root.ResetEvaluate();
            root.Evaluate(param);
            root.TriggerEvaluate();

#if UNITY_EDITOR
            evalParam = param;
#endif
        }

        public void UnActive()
        {
            root.ResetEvaluate();
            root.TriggerEvaluate();

#if UNITY_EDITOR
            evalParam = null;
#endif
        }

        /// <summary>
        /// 重建
        /// </summary>
        public void Rebuild(Bounds bounds, int maxDepth = DEFAULT_DEPTH)
        {
            MaxDepth = maxDepth;

            List<IQuadTreeItem> tempList = itemList;
            itemList = new List<IQuadTreeItem>();

            root = QuadTreeNode.Create(this, bounds, 0);

            if (tempList != null)
            {
                foreach (IQuadTreeItem item in tempList)
                {
                    AddItem(item);
                }
            }
        }

        /// <summary>
        /// 创建四叉树
        /// </summary>
        public static QuadTree Create(Bounds bounds, int maxDepth = DEFAULT_DEPTH)
        {
            return new QuadTree(bounds, maxDepth);
        }

        private QuadTree(Bounds bounds, int maxDepth = 5)
        {
            itemList = new List<IQuadTreeItem>();
            MaxDepth = maxDepth;

            root = QuadTreeNode.Create(this, bounds, 0);
        }

        public void Clear()
        {
            root = null;
            itemList = null;
        }

        /// <summary>
        /// 通过管理对象包围盒调整树节点包围盒高度
        /// </summary>
        /// <param name="delay">延迟一帧执行</param>
        public void AdjustBoundsHeightByItem(bool delay = false)
        {
            if (!EnableHeight)
            {
                return;
            }

            if (delay)
            {
                UpdateManager.Instance.DelayOneFrame(_AdjustBoundsHeightByItem);
            }
            else
            {
                _AdjustBoundsHeightByItem();
            }
        }

        private void _AdjustBoundsHeightByItem()
        {
            root.AdjustBoundsHeightByItem();
        }

#if UNITY_EDITOR
        private object evalParam;
#endif

        /// <summary>
        /// 绘制Gizmos信息
        /// </summary>
        public void DrawGizmos()
        {
            root.DrawGizmos();

#if UNITY_EDITOR
            if (EvalLogic == QuadTreeEvalLogic.EvalByRectArea && evalParam != null)
            {
                Bounds bounds = evalParam.StructConvert<Bounds>();
                Gizmos.color = Color.red;
                Gizmos.DrawWireCube(bounds.center, bounds.size);
            }
#endif
        }
    }
}
