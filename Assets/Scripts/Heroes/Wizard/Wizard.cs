using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Wizard : MonoBehaviour, IHero
{
    [SerializeField] private Image _healthBar;    
    
    public event Action OnAttackStarted;
    public event Action OnAttackEnded;
    public event Action OnTakeHit;
    public event Action OnDeath;

    public bool isDead { get; set; } = false;
    public Side side { get; set; } = Side.Left;
    public Player player { get; set; } = default;
    public HeroStats hero { get; set; }

    private float _currentHealth;
    private float _timeBetweenAttack = 0f;
    private float _attackDuration = 2.1f;
    private bool _isAttackable = false;

    private float _attackInterval = 0.3f;

    private CapsuleCollider2D _attackCollider;
    private BoxCollider2D _bodyCollider;

    private void Awake()
    {
        _attackCollider = GetComponent<CapsuleCollider2D>();
        _bodyCollider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        hero = Instantiate(PlayersManager.Instance.heroes.GetHeroStatsByName("Wizard"));        
        _currentHealth = hero.health;
    }
    private void FixedUpdate()
    {
        if (!_isAttackable) return;
        Attack();
    }

    public void Attack()
    {
        _timeBetweenAttack -= Time.fixedDeltaTime;
        if (_timeBetweenAttack <= 0)
        {
            OnAttackStarted?.Invoke();
            TurnOnColl();

            _timeBetweenAttack = hero.attackRate + _attackDuration;
        }

        /*if(_timeBetweenAttack <= _wizard.attackRate)
            OnAttackEnded?.Invoke();*/

    }
    public void ChangeAttackStatus(bool isAttackable)
    {
        _isAttackable = isAttackable;
    }

    public void TakeHit(float damage)
    {
        OnTakeHit?.Invoke();

        float realDamage = (damage - (damage * (hero.armor / 10)));
        ChangeCurrHealth(realDamage);

        if (_currentHealth <= 0)
        {
            isDead = true;

            OnDeath?.Invoke();
            //_bodyCollider.enabled = false;

            PlayersManager.Instance.FinishBattle();
        }
    }
    public void ChangeCurrHealth(float damage)
    {
        PlayersManager.Instance.TakeHitShowText(gameObject, Mathf.Round(damage));
        _currentHealth -= damage;        
        
        ChangeHealthBar();
    }

    public void TurnOnColl()
    {
        _attackCollider.enabled = false;
        _attackCollider.enabled = true;
    }
    public void TurnOffColl()
    {
        _attackCollider.enabled = false;
    }

    private void ChangeHealthBar()
    {
        _healthBar.fillAmount = _currentHealth / hero.health;
    }

/*    private void OnTriggerStay2D(Collider2D collision)
    {
        _currentAttackInterval -= Time.deltaTime;
        if (_currentAttackInterval <= 0)
        {
            if (_attackCollider.enabled && collision.TryGetComponent<IHero>(out IHero hero) && collision.gameObject != gameObject)
            {
                hero.TakeHit(_wizard.damage);
            }
            _currentAttackInterval = _attackInterval;
        }
        Debug.Log(_currentAttackInterval);
    }*/
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_attackCollider.enabled && collision.TryGetComponent<IHero>(out IHero hero) && collision.gameObject != gameObject)
        {
            Debug.Log("Entered");
            StartCoroutine(PeriodicalDamage(hero));
        }
    }

    private IEnumerator PeriodicalDamage(IHero hero)
    {
        float _currentAttackDuration = 0;
        while (_currentAttackDuration < _attackDuration)
        {            
            hero.TakeHit(this.hero.damage);
            _currentAttackDuration += _attackInterval;

            yield return new WaitForSeconds(_attackInterval);
        }
        DecreaseDamageBuff buff = new DecreaseDamageBuff();
        CorutineManager.Instance.StartCoroutine(buff.ApplyBuff(hero));
        TurnOffColl();
        OnAttackEnded?.Invoke();
        yield return null;
    }
    public void GetStunned(float stunDuration)
    {
        _timeBetweenAttack += stunDuration;
    }
}
