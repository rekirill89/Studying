using UnityEngine;

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
        _archer.OnAttack += _archer_OnAttack;
        _archer.OnTakeHit += _archer_OnTakeHit;
        _archer.OnDeath += _archer_OnDeath;
    }

    private void _archer_OnDeath()
    {
        _animator.SetTrigger(DEATH);
    }

    private void _archer_OnTakeHit()
    {
        _animator.SetTrigger(HIT);
    }

    private void _archer_OnAttack()
    {
        _animator.SetTrigger(ATTACK);
    }

    public void SpawnArrowInAnim()
    {
        _archer.SpawnArrow();
    }
}
