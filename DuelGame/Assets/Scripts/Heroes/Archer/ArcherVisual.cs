using UnityEngine;

namespace DuelGame
{
    public class ArcherVisual : MonoBehaviour
    {
        [SerializeField] private Archer _archer;
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
            _archer.OnAttackVisual += ArcherAttack;
            _archer.OnTakeHitVisual += ArcherTakeHit;
            _archer.OnDeathVisual += ArcherDeath;
        }

        private void ArcherDeath()
        {
            _animator.SetTrigger(DEATH);
        }

        private void ArcherTakeHit()
        {
            _animator.SetTrigger(HIT);
        }

        private void ArcherAttack()
        {
            _animator.SetTrigger(ATTACK);
        }

        public void SpawnArrowInAnim()
        {
            _archer.SpawnArrow();
        }
    }
}

