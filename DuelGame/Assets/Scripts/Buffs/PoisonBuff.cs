using System;
using System.Collections;
using UnityEngine;

namespace DuelGame
{
    public class PoisonBuff : Buff
    {
        [SerializeField] public float poisonPeriodicalDamage;
        [SerializeField] public float interval;

        private float _currenBuffDuration = 0;

        public override IEnumerator Apply(BaseHero hero)
        {
            yield return new WaitForSeconds(0.3f);

            while (_currenBuffDuration < buffDuration)
            {
                hero.ChangeCurrHealth(poisonPeriodicalDamage);
                _currenBuffDuration += interval;

                yield return new WaitForSeconds(interval);
            }
            yield return null;
        }

        public override void DoBuff(BaseHero hero)
        {
            StartCoroutine(Apply(hero));
        }
    }
}

