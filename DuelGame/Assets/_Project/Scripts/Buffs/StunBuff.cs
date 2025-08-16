using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class StunBuff : Buff
    {
        public override float BuffDuration { get; protected set; } = 4f;
        
        public StunBuff(StunBuffConfig config = null)
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
    }
}