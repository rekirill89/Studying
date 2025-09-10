using System;
using UnityEngine;

namespace DuelGame
{
    [RequireComponent(typeof(Animator))]
    public class DeathEffect : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        private const string PLAY = "Play"; 

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void Play()
        {
            _animator.SetTrigger(PLAY);
        }

        public void DestroyObj()
        {
            Destroy(gameObject);
        }
    }
}