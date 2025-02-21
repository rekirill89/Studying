using System;
using System.Collections;
using UnityEngine;

public class PoisonBuff : Buff
{
    public float poisonPeriodicalDamage;
    public float interval;

    private float _currenBuffDuration = 0;
    public PoisonBuff() : base("Poison buff", 2)
    {
        poisonPeriodicalDamage = 3f;
        interval = 0.5f;
    }
/*    public override void ApplyBuff(IHero hero)
    {
        OnBuffAplied?.Invoke();
        StartCorutine
    }*/
    public override IEnumerator ApplyBuff(IHero hero)
    {
        yield return new WaitForSeconds(0.3f);
        PlayersManager.Instance.TakeBuffShowIcon(GetObjOfBuff(), hero.player, buffDuration);
        StartBuff();
        while (_currenBuffDuration < buffDuration)
        {
            hero.ChangeCurrHealth(poisonPeriodicalDamage);
            _currenBuffDuration += interval;

            yield return new WaitForSeconds(interval);
        }
        EndBuff();
        yield return null;
    }

    public override GameObject GetObjOfBuff()
    {
        Debug.Log(nameof(GetObjOfBuff));
        return PlayersManager.Instance.buffs.GetBuffObjByName("Poison");
    }
}
