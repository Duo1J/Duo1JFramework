using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.PhysicsAPI
{
    /// <summary>
    /// 碰撞、触发管理器
    /// </summary>
    public class CollisionManager : MonoSingleton<CollisionManager>, IEditorDrawer
    {
        private Dictionary<int, CollisionController> conDict;

        public CollisionController AddCollision(GameObject go, CollisionType collisionType = CollisionType.Trigger)
        {
            CollisionController con = go.GetOrAddComponent<CollisionController>();
            con.SetCollisionType(collisionType);
            return con;
        }

        public void AddToDict(CollisionController con)
        {
            Assert.NotNull(conDict, "conDict为空");

            int insID = con.gameObject.GetInstanceID();
            if (conDict.ContainsKey(insID))
            {
                Log.ErrorForce($"conDict已包含insID: `{insID}`");
                con.SetEnable(false);
            }
            else
            {
                conDict.Add(insID, con);
            }
        }

        public void RemoveFromDict(CollisionController con)
        {
            Assert.NotNull(conDict, "conDict为空");

            int insID = con.gameObject.GetInstanceID();
            if (conDict.ContainsKey(insID))
            {
                conDict.Remove(insID);
            }
            con.SetEnable(false);
        }

        protected override void OnDispose()
        {
            conDict.Clear();
            conDict = null;
        }

        protected override void OnInit()
        {
            conDict = new Dictionary<int, CollisionController>();
        }

        public void DrawEditorInfo()
        {
            LU.Vertical(() =>
            {
                foreach (CollisionController con in conDict.Values)
                {
                    GUILayout.Space(20);
                    LU.Horizontal(() =>
                    {
                        con.DrawEditorInfo();
                    }, "box");
                }
            });
        }
    }
}