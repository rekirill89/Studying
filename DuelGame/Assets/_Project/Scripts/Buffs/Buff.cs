using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public abstract class Buff
    {
        public float buffDuration {get; private set;}
        public BuffEnum buffEnum {get; protected set;}
        
        public abstract UniTask Execute(BaseHero hero);
        
        protected Buff(float buffDuration)
        {
            this.buffDuration = buffDuration;
        }

        
    }
}

