using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public class DecreaseDamageBuff : Buff
    {        
        public override float BuffDuration { get; protected set; } = 5f;

        private float _decreaseDamageMultiplier = 3f;
        
        public DecreaseDamageBuff(DecreaseDamageBuffRemoteConfig config = null)
        {
            BuffEnum = BuffEnum.DecreaseDamage;
            
            if(config == null)
                return;
            
            BuffDuration = config.Duration;
            _decreaseDamageMultiplier = config.DamageMultiplier;
        }

        public override async UniTask Execute(BaseHero target)
        {
            await base.Execute(target);
        
            var defaultDamage = target.Hero.Damage;
            target.Hero.Damage = defaultDamage - (defaultDamage / _decreaseDamageMultiplier);

            await UniTask.Delay(TimeSpan.FromSeconds(BuffDuration), cancellationToken: _token);
            target.Hero.Damage = defaultDamage;
        }

        /*public void SetBuffStats(DecreaseDamageBuffRemoteConfig config)
        {
            BuffDuration = config.Duration;
            _decreaseDamageMultiplier = config.DamageMultiplier;
        }*/
    }
}