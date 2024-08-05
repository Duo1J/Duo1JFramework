using System;
using System.Collections.Generic;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    public class ActorManager : MonoSingleton<ActorManager>
    {
        /// <summary>
        /// 主角
        /// </summary>
        public BaseActor MainActor { get; private set; }

        /// <summary>
        /// 已创建Actor字典
        /// </summary>
        private Dictionary<long, BaseActor> actorDict;

        private AutoIncID incID;

        /// <summary>
        /// 创建Actor
        /// </summary>
        public BaseActor CreateActor(ActorData actorData, bool autoCreate = true)
        {
            Type logicType = actorData.LogicType;
            try
            {
                BaseActor actor = Activator.CreateInstance(logicType) as BaseActor;
                actor.Init(incID.NewID, actorData);
                if (autoCreate)
                {
                    actor.Create();
                }
                actorDict.Add(actor.ID, actor);
                return actor;
            }
            catch (Exception e)
            {
                Assert.ExceptHandle(e, $"创建Actor失败: {actorData.ToString()}");
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
            if (MainActor != null)
            {
                MainActor.UnBindCamera();
            }

            MainActor = actor;

            if (bindCamera)
            {
                MainActor.BindCamera();
            }
        }

        protected override void OnDispose()
        {
            RemoveAllActor();
        }

        protected override void OnInit()
        {
            incID = AutoIncID.Create();
            actorDict = new Dictionary<long, BaseActor>();
        }
    }
}