using UnityEngine;

namespace DuelGame
{
    public class WarriorVisual : MonoBehaviour
    {
        [SerializeField] private Warrior _warrior;
        private Animator _animator;

        private const string ATTACK = "Attack";
        private const string HIT = "Hit";
        private const string DEATH = "Death";

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }
        private void Start()
        {
            _warrior.OnAttackVisual += WarriorAttack;
            _warrior.OnTakeHitVisual += WarriorTakeHit;
            _warrior.OnDeathVisual += WarriorDeath;
        }

        private void WarriorDeath()
        {
            _animator.SetTrigger(DEATH);
        }

        private void WarriorTakeHit()
        {
            _animator.SetTrigger(HIT);
        }

        private void WarriorAttack()
        {
            _animator.SetTrigger(ATTACK);
        }
        public void TurnOnCollAnim()
        {
            _warrior.TurnOnColl();
        }
        public void TurnOffCollAnim()
        {
            _warrior.TurnOffColl();
        }
    }
}

