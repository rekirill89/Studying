using System;
using System.Collections;
using UnityEngine;

public class PlayersManager : MonoBehaviour
{    
    [SerializeField] public HeroesList heroes;
    [SerializeField] public BuffsList buffs;

    [SerializeField] private Transform _firstPlayerTrans;
    [SerializeField] private Transform _secondPlayerTrans;


    private GameObject _player1;
    private GameObject _player2;


    public static PlayersManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        GameManager.Instance.OnGameStarted += RunGameProcess;
        GameManager.Instance.OnGameContinued += Instance_OnGameContinued;
        //GameManager.Instance.OnGameEnded += Instance_OnGameEnded;
    }

    private void Instance_OnGameContinued()
    {
        Destroy(_player1);
        Destroy(_player2);
        RunGameProcess();
    }

    public void FinishBattle()
    {
        ChangeAttackStatusToPlayers(false);
        if (_player1.GetComponent<IHero>().isDead)
        {
            GameManager.Instance.GameFinishPanel();
        }
        else
        {
            GameManager.Instance.GameContinuePanel();
        }
    }

    public void RunGameProcess()
    {
        _player1 = Instantiate(heroes.GetRandomHero(), _firstPlayerTrans.position, Quaternion.identity);
        _player2 = Instantiate(heroes.GetRandomHero(), _secondPlayerTrans.position, Quaternion.identity);

        _player1.GetComponent<IHero>().ChoosSideAndPlayer(Side.Left, Player.Player1, _player1.transform);
        _player2.GetComponent<IHero>().ChoosSideAndPlayer(Side.Right, Player.Player2, _player2.transform);

        _player1.transform.SetParent(_firstPlayerTrans);
        _player2.transform.SetParent(_secondPlayerTrans);

        Debug.Log(_player1.GetHashCode());
        Debug.Log(_player2.GetHashCode());

        StartCoroutine(SpawnHero(_player1, 2f));
        StartCoroutine(SpawnHero(_player2, 3f));
    }
    private IEnumerator SpawnHero(GameObject obj, float timeToAttack)
    {
        yield return new WaitForSeconds(timeToAttack);
        obj.GetComponent<IHero>().ChangeAttackStatus(true);
    }
    public void ChangeAttackStatusToPlayers(bool isAttackable)
    {
        _player1.GetComponent<IHero>().ChangeAttackStatus(isAttackable);
        _player2.GetComponent<IHero>().ChangeAttackStatus(isAttackable);
    }

    public void TakeHitShowText(GameObject hero, float damage)
    {
        if(hero == _player1)
            UIManager.Instance.SpawnDamageText(damage, Player.Player1);
        else if(hero == _player2)
            UIManager.Instance.SpawnDamageText(damage, Player.Player2);
    }

    public void TakeBuffShowIcon(GameObject icon, Player player, float duration)
    {
        Debug.Log($"{player}, {_player1.GetComponent<IHero>().player}, {_player2.GetComponent<IHero>().player}");
        Debug.Log(nameof(TakeBuffShowIcon));
        if (player == _player1.GetComponent<IHero>().player)
        {
            Debug.Log(1);
            CorutineManager.Instance.StartCoroutine(UIManager.Instance.SpawnBuffIcon(icon, duration, Player.Player1));
        }

        else if (player == _player2.GetComponent<IHero>().player)
        {
            Debug.Log(2);
            CorutineManager.Instance.StartCoroutine(UIManager.Instance.SpawnBuffIcon(icon, duration, Player.Player2));
        }

    }
}
public enum Player
{
    Player1,
    Player2
}
