using System;
using System.Collections.Generic;
using System.Linq;

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
            if (actorData == null)
            {
                Log.ErrorForce("创建Actor失败: ActorData为空");
                return null;
            }

            Type logicType = actorData.LogicType;
            if (logicType == null || !typeof(BaseActor).IsAssignableFrom(logicType))
            {
                Log.ErrorForce($"创建Actor失败: LogicType无效，{actorData.ToString()}");
                return null;
            }

            try
            {
                BaseActor actor = Activator.CreateInstance(logicType) as BaseActor;
                if (actor == null)
                {
                    Log.ErrorForce($"创建Actor失败: LogicType无法转换BaseActor，{actorData.ToString()}");
                    return null;
                }

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
            BaseActor[] actors = actorDict.Values.ToArray();
            actorDict.Clear();
            foreach (BaseActor actor in actors)
            {
                actor.Dispose();
            }
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
