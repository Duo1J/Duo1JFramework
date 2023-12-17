using System;
using System.Collections.Generic;
using UObject = UnityEngine.Object;

namespace Duo1JFramework.TimerUpdate
{
    public class UpdateManager : MonoSingleton<UpdateManager>
    {
        private Dictionary<int, Action> updateDict;

        /// <summary>
        /// 注册Update
        /// </summary>
        public void Register(UObject obj, Action updater)
        {
            updateDict.Add(obj.GetInstanceID(), updater);
        }

        /// <summary>
        /// 取消注册Update
        /// </summary>
        public void UnRegister(UObject obj, Action updater)
        {
            int insID = obj.GetInstanceID();
            if (updateDict.ContainsKey(insID))
            {
                updateDict.Remove(insID);
            }
        }

        private void Update()
        {
            if (updateDict != null)
            {
                foreach (KeyValuePair<int, Action> kv in updateDict)
                {
                    if (kv.Value == null)
                    {
                        Log.Error($"Update注册字典中，Key: {kv.Key} 对应的回调为空");
                        updateDict.Remove(kv.Key);
                    }
                    else
                    {
                        kv.Value.Invoke();
                    }
                }
            }
        }

        protected override void OnDispose()
        {
            updateDict = null;
        }

        protected override void OnInit()
        {
            updateDict = new Dictionary<int, Action>();
        }
    }
}