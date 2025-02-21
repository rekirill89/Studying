using System;
using UnityEngine;
using UnityEngine.UI;

public class Archer : MonoBehaviour, IHero
{
    [SerializeField] private Transform _arrowTrans;
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Image _healthBar;

    public event Action OnAttack;
    public event Action OnTakeHit;
    public event Action OnDeath;

    public bool isDead { get; set; } = false;
    public Side side { get; set; } = Side.Left;
    public Player player { get; set; } = default;
    public HeroStats hero { get; set; }

    private float _currentHealth;
    private float _timeBetweenAttack = 0f;
    private bool _isAttackable = false;

    //private PoisonBuff _poisonBuff;
    private BoxCollider2D _bodyCollider;
    private void Awake()
    {
        _bodyCollider = GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        //_poisonBuff = new PoisonBuff();
        hero = Instantiate(PlayersManager.Instance.heroes.GetHeroStatsByName("Archer"));
        _currentHealth = hero.health;
        _currentHealth = hero.health;
        _healthBar.fillAmount = 1;
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
            OnAttack?.Invoke();

            _timeBetweenAttack = hero.attackRate;
        }
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

    public void SpawnArrow()
    {
        var x = Instantiate(_arrow, _arrowTrans.position, Quaternion.identity);

        if(side == Side.Right)
            x.transform.eulerAngles = new Vector3(0, 0, 180);
        x.GetComponent<Arrow>().Initialize(hero.damage, new PoisonBuff());
    }
    private void ChangeHealthBar()
    {
        _healthBar.fillAmount = _currentHealth / hero.health;
    }
    public void GetStunned(float stunDuration)
    {
        _timeBetweenAttack += stunDuration;
    }
}
