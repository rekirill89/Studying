using System.Collections;
using UnityEngine;

namespace DuelGame
{
    public class StunBuff : Buff
    {    
        public override IEnumerator Apply(BaseHero hero)
        {
            hero.GetStunned(4);

            yield return null;
        }

        public override void DoBuff(BaseHero hero)
        {
            StartCoroutine(Apply(hero));
        }
    }
}

