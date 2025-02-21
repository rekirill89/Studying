using System;
using System.Collections;
using UnityEngine;

public abstract class Buff
{
    public string buffName;
    public float buffDuration;

    public event Action OnBuffApplied;
    public event Action OnBuffEnded;


    public Buff(string name, float duration)
    {
        buffName = name;
        buffDuration = duration;
    }

    public abstract IEnumerator ApplyBuff(IHero hero);

    public abstract GameObject GetObjOfBuff();    

    public virtual void StartBuff()
    {
        OnBuffApplied?.Invoke();
    }
    public virtual void EndBuff()
    {
        OnBuffEnded?.Invoke();
    }
}
