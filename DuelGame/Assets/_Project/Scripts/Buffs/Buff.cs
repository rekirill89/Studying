using System;
using System.Collections;
using UnityEngine;

namespace DuelGame
{
    public abstract class Buff
    {
        public float BuffDuration {get; private set;}
        
        protected Buff(float buffDuration)
        {
            BuffDuration = buffDuration;
        }

        public abstract void DoBuff(BaseHero hero);
    }
}

