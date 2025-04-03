using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class WizardVisual : BaseHeroVisual
    {
        private Wizard _wizard;

        private bool _attacking = false;

        protected override void Awake()
        {
            base.Awake();

            _wizard = _hero as Wizard;
        }
              
        private void Update()
        {
            AttackAnim();
        }
  
        /*
        private void Start()
        {
            SubscribeToEvents();
        }*/
        
        protected override void SubscribeToEvents()
        {
            _wizard.OnAttack += WizzardAttackStarted;
            _wizard.OnAttackEnded += WizzardAttackEnded;

            _hero.OnTakeDamage += ShowDamageText;
            _hero.OnTakeHit += HeroTakeHit;
            _hero.OnDeath += HeroDeath;
            _hero.OnHealthChanged += ChangeHealthBar;

            _hero.OnBuffApplied += HeroBuffApplied;
        }
        
        protected override void UnsubscribeFromEvents()
        {
            _wizard.OnAttack -= WizzardAttackStarted;
            _wizard.OnAttackEnded -= WizzardAttackEnded;

            _hero.OnTakeDamage -= ShowDamageText;
            _hero.OnTakeHit -= HeroTakeHit;
            _hero.OnDeath -= HeroDeath;
            _hero.OnHealthChanged -= ChangeHealthBar;

            _hero.OnBuffApplied -= HeroBuffApplied;
        }

        private void WizzardAttackStarted()
        {
            _attacking = true;
            _wizard.TurnOnColl();
        }
        
        private void WizzardAttackEnded()
        {
            _attacking = false;
            _wizard.TurnOffColl();
        }

        private void AttackAnim()
        {
            _animator.SetBool(ATTACK, _attacking);
        }
    }
}
