using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace DuelGame
{
    public abstract class Buff
    {
        public BuffEnum BuffEnum {get; protected set;}
        protected float BuffDuration {get; private set;}
        
        protected Buff(float buffDuration)
        {
            this.BuffDuration = buffDuration;
        }
        
        public virtual UniTask Execute(BaseHero target, Sprite sprite)
        {
            target.BuffAppliedInvoke(sprite, BuffDuration);
            return UniTask.CompletedTask;
        }
    }
}