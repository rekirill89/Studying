using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Wizard : BaseHero
    {
        public event Action OnAttackEnded;

        public override HeroEnum heroEnum { get; } = HeroEnum.Wizard;

        private readonly float _attackDuration = 2.1f;
        private readonly float _attackInterval = 0.3f;
        private readonly float _attackIntervalTimer = 0.25f;
        
        private void Awake()
        {
            BodyCollider = GetComponent<BoxCollider2D>();
        }

        public override void DamageEnemy()
        {
            StartTickableDamage(EnemyHero).Forget();
        }

        private async UniTask StartTickableDamage(BaseHero hero)
        {
            float currentAttackDuration = 0;
            IsAttackable = false;
            while (currentAttackDuration < _attackDuration && !Cts.IsCancellationRequested)
            {
                hero.TakeHit(Hero.Damage);
                currentAttackDuration += _attackInterval;

                await UniTask.Delay(TimeSpan.FromSeconds(_attackIntervalTimer), cancellationToken: Cts.Token);
            }

            hero.TakeHit(Hero.Damage);
            InvokeApplyBuffToEnemy(hero);
            IsAttackable = true;
            OnAttackEnded?.Invoke();
        }
    }
}