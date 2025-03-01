using System;
using UnityEngine;

namespace DuelGame
{
    public class WizardVisual : MonoBehaviour
    {
        [SerializeField] private Wizard _wizard;
        private Animator _animator;

        private const string ATTACK = "Attacking";
        private const string HIT = "Hit";
        private const string DEATH = "Death";

        private bool _attacking = false;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        private void Update()
        {
            AttackAnim();
        }

        private void Start()
        {
            _wizard.OnAttack += WizzardAttackStarted;
            _wizard.OnAttackEnded += WizzardAttackEnded;

            _wizard.OnTakeHitVisual += WizardTakeHit;
            _wizard.OnDeathVisual += WizardDeath;
        }

        private void WizardDeath()
        {
            _animator.SetTrigger(DEATH);
        }

        private void WizardTakeHit()
        {
            _animator.SetTrigger(HIT);
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
