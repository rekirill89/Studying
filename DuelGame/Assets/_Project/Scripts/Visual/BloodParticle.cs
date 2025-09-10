using System;
using UnityEngine;

namespace DuelGame
{
    [RequireComponent(typeof(ParticleSystem))]
    public class BloodParticle : MonoBehaviour
    {
        public ParticleSystem Ps {get; private set;}

        private void Awake()
        {
            Ps = GetComponent<ParticleSystem>();
        }

        public void Init(Vector3 position)
        {
            transform.position = position;
        }
    }
}