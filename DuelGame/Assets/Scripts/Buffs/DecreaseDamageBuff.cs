using System.Collections;
using UnityEngine;

namespace DuelGame
{
    public class DecreaseDamageBuff : Buff
    {
        public override IEnumerator Apply(BaseHero hero)
        {
            float defaultDamage = hero.hero.damage;
            hero.hero.damage = defaultDamage - (defaultDamage / 3);

            yield return new WaitForSeconds(buffDuration);

            hero.hero.damage = defaultDamage;
        }

        public override void DoBuff(BaseHero hero)
        {
            StartCoroutine(Apply(hero));
        }
    }
}
