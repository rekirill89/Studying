using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class StunBuff : Buff
    {
        public override float BuffDuration { get; protected set; } = 4f;
        
        public StunBuff(StunBuffRemoteConfig config = null)
        {
            BuffEnum = BuffEnum.Stun;
            
            if(config == null)
                return;
            
            BuffDuration = config.Duration;
        }

        public override async UniTask Execute(BaseHero target)
        {
            await base.Execute(target);
            
            target.GetStunned(BuffDuration);
            await UniTask.Yield();
        }
        
        /*public void SetBuffStats(StunBuffRemoteConfig config)
        {
            BuffDuration = config.Duration;
        }*/
    }
}