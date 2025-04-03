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
        private readonly float _decreaseDamageMultiplier = 3f;
        
        public DecreaseDamageBuff(CancellationToken token) :base(5)
        {
            _token = token;
            buffEnum = BuffEnum.DecreaseDamage;
        }
        
        public override async UniTask Execute(BaseHero target)
        {
            var defaultDamage = target.hero.damage;
            target.hero.damage = defaultDamage - (defaultDamage / _decreaseDamageMultiplier);

            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(buffDuration), cancellationToken: _token);
            }
            finally
            {
                target.hero.damage = defaultDamage;
            }
        }
    }
}
