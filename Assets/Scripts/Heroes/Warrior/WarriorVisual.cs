using UnityEngine;

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
    private void Update()
    {

    }
    private void Start()
    {
        _warrior.OnAttack += _warrior_OnAttack;
        _warrior.OnTakeHit += _warrior_OnTakeHit;
        _warrior.OnDeath += _warrior_OnDeath;
    }

    private void _warrior_OnDeath()
    {
        _animator.SetTrigger(DEATH);
    }

    private void _warrior_OnTakeHit()
    {
        _animator.SetTrigger(HIT);
    }

    private void _warrior_OnAttack()
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
