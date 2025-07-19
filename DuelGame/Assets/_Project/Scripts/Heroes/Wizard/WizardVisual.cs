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

            _wizard = Hero as Wizard;
        }
              
        private void Update()
        {
            AttackAnim();
        }
        
        protected override void SubscribeToEvents()
        {
            _wizard.OnAttack += WizardAttackStartedAnim;
            _wizard.OnAttackEnded += WizardAttackEndAnim;
            
            Hero.OnPlayerStop += StopAllTasks;
            Hero.OnTakeDamage += DamageTaken;
            Hero.OnDeath += HeroDeath;
        }
        
        protected override void UnsubscribeFromEvents()
        {
            _wizard.OnAttack -= WizardAttackStartedAnim;
            _wizard.OnAttackEnded -= WizardAttackEndAnim;
            
            Hero.OnPlayerStop -= StopAllTasks;
            Hero.OnTakeDamage -= DamageTaken;
            Hero.OnDeath -= HeroDeath;
        }

        private void WizardAttackStartedAnim()
        {
            _attacking = true;
            _wizard.DamageEnemy();
        }
        
        private void WizardAttackEndAnim()
        {
            _attacking = false;
        }

        private void AttackAnim()
        {
            Animator.SetBool(ATTACK, _attacking);
        }
    }
}