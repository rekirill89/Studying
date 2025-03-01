using System;
using System.Collections;
using UnityEngine;

namespace DuelGame
{
    public abstract class Buff : MonoBehaviour
    {
        [SerializeField] public float buffDuration;

        public abstract IEnumerator Apply(BaseHero hero);
        public abstract void DoBuff(BaseHero hero);
    }
}

