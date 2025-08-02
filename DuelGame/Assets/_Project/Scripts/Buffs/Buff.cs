using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public abstract class Buff
    {
        public BuffEnum BuffEnum {get; protected set;}
        public abstract float BuffDuration {get; protected set;}

        protected CancellationToken Token; 
        
        public virtual UniTask Execute(BaseHero target)
        {
            return UniTask.CompletedTask;
        }

        public void SetCtsToken(CancellationToken ct)
        {
            Token = ct;
        }
    }
}