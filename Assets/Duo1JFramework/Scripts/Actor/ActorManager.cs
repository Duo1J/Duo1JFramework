using Duo1JFramework.Camera3D;
using System;
using System.Collections.Generic;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    public class ActorManager : MonoSingleton<ActorManager>
    {
        public BaseActor mainActor;

        private Dictionary<long, BaseActor> actorDict;

        private AutoIncID IncID = AutoIncID.Create();

        public BaseActor CreateActor(ActorData actorData, bool create = true)
        {
            Type logicType = actorData.LogicType;
            try
            {
                BaseActor actor = Activator.CreateInstance(logicType) as BaseActor;
                actor.Init(IncID.NewID, actorData);
                if (create) actor.Create();
                actorDict.Add(actor.ID, actor);
                return actor;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, "创建Actor失败");
                return null;
            }
        }

        /// <summary>
        /// 移除Actor
        /// </summary>
        public void RemoveActor(BaseActor actor)
        {
            RemoveActor(actor.ID);
        }

        /// <summary>
        /// 通过ID移除Actor
        /// </summary>
        public void RemoveActor(long id)
        {
            if (actorDict.TryGetValue(id, out BaseActor actor))
            {
                actor.Dispose();
                actorDict.Remove(id);
            }
        }

        /// <summary>
        /// 移除所有Actor
        /// </summary>
        public void RemoveAllActor()
        {
            foreach (BaseActor actor in actorDict.Values)
            {
                actor.Dispose();
            }
            actorDict = new Dictionary<long, BaseActor>();
        }

        /// <summary>
        /// 设置主角
        /// </summary>
        public void SetMainActor(BaseActor actor, bool bindCamera = false)
        {
            if (mainActor != null) mainActor.UnBindCamera();
            mainActor = actor;
            if (bindCamera) mainActor.BindCamera();
        }

        protected override void OnDispose()
        {
            RemoveAllActor();
        }

        protected override void OnInit()
        {
            actorDict = new Dictionary<long, BaseActor>();
        }
    }
}