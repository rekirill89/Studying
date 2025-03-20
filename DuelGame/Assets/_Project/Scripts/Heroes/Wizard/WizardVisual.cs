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

            _wizard = hero as Wizard;
        }
        
        protected override void SubscribeToEvents()
        {
            _wizard.OnAttack += WizzardAttackStarted;
            _wizard.OnAttackEnded += WizzardAttackEnded;

            hero.OnTakeDamage += ShowDamageText;
            hero.OnTakeHit += HeroTakeHit;
            hero.OnDeath += HeroDeath;
            hero.OnHealthChanged += ChangeHealthBar;


            hero.OnBuffAplied += HeroBuffAplied;
        }

        
        private void Update()
        {
            AttackAnim();
        }

        private void Start()
        {
            SubscribeToEvents();
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
