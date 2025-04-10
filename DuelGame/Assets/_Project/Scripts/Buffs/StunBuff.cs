using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class StunBuff : Buff
    {
        private const float STUN_BUFF_DURATION = 4f;

        public StunBuff() : base(STUN_BUFF_DURATION)
        {
            BuffEnum = BuffEnum.Stun;
        }
        
        public override async UniTask Execute(BaseHero target, Sprite sprite)
        {
            await base.Execute(target, sprite);
            try
            {
                target.GetStunned(BuffDuration);
                await UniTask.Yield();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}

