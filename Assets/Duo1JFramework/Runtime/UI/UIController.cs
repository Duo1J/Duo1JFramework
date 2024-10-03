using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.UI
{
    /// <summary>
    /// UI控制器 (挂载在UI预制体根节点)
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class UIController : BaseMono
    {
        #region Field

        /// <summary>
        /// 画布
        /// </summary>
        private Canvas canvas;

        /// <summary>
        /// 节点列表
        /// </summary>
        [SerializeField]
        private List<Transform> nodeList;

        /// <summary>
        /// 节点字典
        /// </summary>
        private Dictionary<string, Transform> nodeDict;

        /// <summary>
        /// 组件字典
        /// </summary>
        private Dictionary<string, MonoBehaviour> comDict;

        /// <summary>
        /// 排序层级
        /// </summary>
        public int SortingOrder
        {
            get => canvas.sortingOrder;
            set
            {
                canvas.overrideSorting = true;
                canvas.sortingOrder = value;
            }
        }

        #endregion Field

        #region Public Method

        /// <summary>
        /// 获取GameObject
        /// </summary>
        public GameObject GetGo(string goName)
        {
            if (string.IsNullOrEmpty(goName))
            {
                return gameObject;
            }

            if (nodeDict.TryGetValue(goName, out Transform tf))
            {
                return tf.gameObject;
            }

            Log.Error($"未找到名称为: {goName} 的Go");
            return null;
        }

        /// <summary>
        /// 获取组件
        /// </summary>
        public T GetCom<T>(string goName) where T : MonoBehaviour
        {
            string comDictKey = goName + typeof(T).Name;
            if (comDict.TryGetValue(comDictKey, out MonoBehaviour mb))
            {
                return mb.Convert<T>();
            }

            GameObject go = GetGo(goName);
            if (go == null)
            {
                return default(T);
            }

            T com = go.GetComponent<T>();
            if (com == null)
            {
                Log.Error($"未在 {goName} 找到类型为: {typeof(T)} 的Com");
                return default(T);
            }

            comDict.Add(comDictKey, com);
            return com;
        }

        #endregion Public Method

        private void Awake()
        {
            BuildNodeDict();
            canvas = GetComponent<Canvas>();
        }

        #region 节点收集

        /// <summary>
        /// 获取节点列表
        /// </summary>
        public List<Transform> GetNodeList()
        {
            return nodeList;
        }

        /// <summary>
        /// 收集节点
        /// </summary>
        public void CollectNode()
        {
            nodeList = new List<Transform>();

            Dictionary<string, Transform> checkDict = new Dictionary<string, Transform>();
            Queue<Transform> queue = new Queue<Transform>();
            queue.Enqueue(transform);

            while (queue.Count > 0)
            {
                Transform tf = queue.Dequeue();
                if (tf.name.StartsWith(Def.UI.NODE_PREFIX))
                {
                    if (checkDict.ContainsKey(tf.name))
                    {
                        Log.Error($"重复名称的节点: {tf.name}");
                    }
                    else
                    {
                        nodeList.Add(tf);
                        checkDict.Add(tf.name, tf);
                    }
                }
                for (int i = 0, len = tf.childCount; i < len; i++)
                {
                    queue.Enqueue(tf.GetChild(i));
                }
            }
        }

        /// <summary>
        /// 构建节点字典
        /// </summary>
        public void BuildNodeDict()
        {
            nodeDict = new Dictionary<string, Transform>();
            comDict = new Dictionary<string, MonoBehaviour>();

            if (nodeList == null) return;

            for (int i = 0; i < nodeList.Count; i++)
            {
                Transform node = nodeList[i];
                if (node != null)
                {
                    nodeDict.Add(node.name.Substring(2), node);
                }
            }
        }

        #endregion 节点收集
    }
}
