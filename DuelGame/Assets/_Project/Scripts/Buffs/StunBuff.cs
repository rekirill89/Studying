using System.Collections;
using UnityEngine;

namespace DuelGame
{
    public class StunBuff : Buff
    {
        public StunBuff() : base(4){}
        
        public override void DoBuff(BaseHero hero)
        {
            hero.GetStunned(BuffDuration);
        }
    }
}

