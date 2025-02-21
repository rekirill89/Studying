using System.Collections;
using UnityEngine;

public class StunBuff : Buff
{    
    public StunBuff() : base("Stun buff", 2) { }

    public override IEnumerator ApplyBuff(IHero hero)
    {
        //yield return new WaitForSeconds(0.3f);
        PlayersManager.Instance.TakeBuffShowIcon(GetObjOfBuff(), hero.player, buffDuration);
        StartBuff();
        hero.GetStunned(4);
        EndBuff();
        yield return null;
    }

    public override GameObject GetObjOfBuff()
    {
        Debug.Log(nameof(GetObjOfBuff));
        return PlayersManager.Instance.buffs.GetBuffObjByName("Stun");
    }
}
