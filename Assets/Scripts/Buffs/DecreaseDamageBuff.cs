using System.Collections;
using UnityEngine;

public class DecreaseDamageBuff : Buff
{
    public DecreaseDamageBuff() : base("Decrease damage buff", 5) { }    
    
    public override IEnumerator ApplyBuff(IHero hero)
    {
        //yield return new WaitForSeconds(0.3f);
        PlayersManager.Instance.TakeBuffShowIcon(GetObjOfBuff(), hero.player, buffDuration);
        StartBuff();

        float defaultDamage = hero.hero.damage;
        hero.hero.damage = defaultDamage - (defaultDamage / 3);

        yield return new WaitForSeconds(buffDuration);
        EndBuff();

        hero.hero.damage = defaultDamage;
    }

    public override GameObject GetObjOfBuff()
    {
        Debug.Log(nameof(GetObjOfBuff));
        return PlayersManager.Instance.buffs.GetBuffObjByName("Decrease damage");
    }
}
