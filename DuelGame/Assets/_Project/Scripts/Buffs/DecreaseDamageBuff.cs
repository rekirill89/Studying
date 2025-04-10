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
        private const float DECREASE_DAMAGE_BUFF_DURATION = 5f;
        
        public DecreaseDamageBuff(CancellationToken token) :base(DECREASE_DAMAGE_BUFF_DURATION)
        {
            _token = token;
            BuffEnum = BuffEnum.DecreaseDamage;
        }
        
        public override async UniTask Execute(BaseHero target, Sprite sprite)
        {
            try
            {
                await base.Execute(target, sprite);
            
                var defaultDamage = target.Hero.Damage;
                target.Hero.Damage = defaultDamage - (defaultDamage / _decreaseDamageMultiplier);

                try
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(BuffDuration), cancellationToken: _token);
                }
                finally
                {
                    target.Hero.Damage = defaultDamage;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
    }
}
