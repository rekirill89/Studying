using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class DecreaseDamageBuff : Buff
    {
        private readonly CancellationToken _token;

        public DecreaseDamageBuff(CancellationToken token) :base(5)
        {
            _token = token;
        }
        
        public override void DoBuff(BaseHero hero)
        {
            //StartCoroutine(Apply(hero));
            Apply(hero).Forget();
        }

        
        private async UniTask Apply(BaseHero target)
        {
            var defaultDamage = target.hero.damage;
            target.hero.damage = defaultDamage - (defaultDamage / 3);

            await UniTask.Delay(TimeSpan.FromSeconds(BuffDuration), cancellationToken: _token);

            target.hero.damage = defaultDamage;
        }
    }
}
