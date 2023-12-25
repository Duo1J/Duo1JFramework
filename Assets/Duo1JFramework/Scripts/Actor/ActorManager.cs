using System.Collections.Generic;
using UnityEngine;

namespace Duo1JFramework.Actor
{
    /// <summary>
    /// 角色管理器
    /// </summary>
    public class ActorManager : MonoSingleton<ActorManager>
    {
        Dictionary<int, BaseActor> actorDict;

        public BaseActor CreateActor()
        {
            //todo 
            return null;
        }

        public void RemoveActor(BaseActor actor)
        {

        }

        protected override void OnDispose()
        {
        }

        protected override void OnInit()
        {
            actorDict = new Dictionary<int, BaseActor>();
        }
    }
}