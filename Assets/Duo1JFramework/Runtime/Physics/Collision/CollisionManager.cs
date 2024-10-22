using Duo1JFramework.PhysicsAPI.Physics2D;
using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.PhysicsAPI
{
    /// <summary>
    /// 碰撞、触发管理器
    /// </summary>
    public class CollisionManager : MonoSingleton<CollisionManager>, IEditorDrawer
    {
        /// <summary>
        /// 碰撞控制器字典
        /// </summary>
        private Dictionary<int, ICollisionController> conDict;

        /// <summary>
        /// 为Go添加3D碰撞
        /// </summary>
        public CollisionController AddCollision(GameObject go, ECollisionType collisionType = ECollisionType.Trigger)
        {
            CollisionController con = go.GetOrAddComponent<CollisionController>();
            con.SetCollisionType(collisionType);
            return con;
        }

        /// <summary>
        /// 为Go添加2D碰撞
        /// </summary>
        public CollisionController2D AddCollision2D(GameObject go, ECollisionType collisionType = ECollisionType.Trigger)
        {
            CollisionController2D con = go.GetOrAddComponent<CollisionController2D>();
            con.SetCollisionType(collisionType);
            return con;
        }

        /// <summary>
        /// 添加控制器到字典管理
        /// </summary>
        public void AddToDict(ICollisionController con)
        {
            int insID = con.GetInstanceID();
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

        /// <summary>
        /// 从字典移除控制器
        /// </summary>
        /// <param name="con"></param>
        public void RemoveFromDict(ICollisionController con)
        {
            int insID = con.GetInstanceID();
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
            conDict = new Dictionary<int, ICollisionController>();
        }

        public void DrawEditorInfo()
        {
            ED.Vertical(() =>
            {
                foreach (ICollisionController con in conDict.Values)
                {
                    GUILayout.Space(20);
                    ED.Horizontal(() =>
                    {
                        con.DrawEditorInfo();
                    }, "box");
                }
            });
        }
    }
}
