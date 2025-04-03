using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class StunBuff : Buff
    {
        public StunBuff() : base(4)
        {
            buffEnum = BuffEnum.Stun;
        }
        
        public override async UniTask Execute(BaseHero hero)
        {
            hero.GetStunned(buffDuration);
            await UniTask.Yield();
        }
    }
}

