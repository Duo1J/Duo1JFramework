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

        private Canvas canvas;

        [SerializeField]
        private List<Transform> nodeList;
        private Dictionary<string, Transform> nodeDict;
        private Dictionary<string, MonoBehaviour> comDict;
        public const string NodePrefix = "@_";

        #endregion

        private void Awake()
        {
            BuildNodeDict();
            canvas = GetComponent<Canvas>();
        }

        #region Public

        /// <summary>
        /// 更新此窗口及其子物体的层级
        /// </summary>
        public void UpdateLayer(int layer)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = layer;
        }

        public GameObject GetGo(string goName)
        {
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

        #endregion Public


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
                if (tf.name.StartsWith(NodePrefix))
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

            foreach (Transform tf in nodeList)
            {
                nodeDict.Add(tf.name.Substring(2), tf);
            }
        }

        #endregion 节点收集
    }
}