using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class DecreaseDamageBuff : Buff
    {        
        private const float DECREASE_DAMAGE_BUFF_DURATION = 5f;
                
        private readonly float _decreaseDamageMultiplier = 3f;
        
        private readonly CancellationToken _token;
        
        public DecreaseDamageBuff(CancellationToken token) :base(DECREASE_DAMAGE_BUFF_DURATION)
        {
            _token = token;
            BuffEnum = BuffEnum.DecreaseDamage;
        }
        
        public override async UniTask Execute(BaseHero target/*, Sprite sprite*/)
        {
            await base.Execute(target/*, sprite*/);
        
            var defaultDamage = target.Hero.Damage;
            target.Hero.Damage = defaultDamage - (defaultDamage / _decreaseDamageMultiplier);

            await UniTask.Delay(TimeSpan.FromSeconds(BuffDuration), cancellationToken: _token);
            target.Hero.Damage = defaultDamage;
        }
    }
}