using System;
using UnityEngine;

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
        _wizard.OnAttackStarted += _warrior_OnAttackStarted;
        _wizard.OnAttackEnded += _warrior_OnAttackEnded;

        _wizard.OnTakeHit += _wizard_OnTakeHit;
        _wizard.OnDeath += _wizard_OnDeath;
    }

    private void _wizard_OnDeath()
    {
        _animator.SetTrigger(DEATH);
    }

    private void _wizard_OnTakeHit()
    {
        _animator.SetTrigger(HIT);
    }

    private void _warrior_OnAttackStarted()
    {
        _attacking = true;
        //_wizard.TurnOnColl();
    }
    private void _warrior_OnAttackEnded()
    {
        _attacking = false;
        //_wizard.TurnOffColl();
    }

    private void AttackAnim()
    {
        _animator.SetBool(ATTACK, _attacking);
    }
}
