using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DuelGame
{
    public class Archer : BaseHero
    {        
        [SerializeField] private Transform arrowTrans;
        [SerializeField] private Arrow arrow;

        private void Awake()
        {
            _bodyCollider = GetComponent<BoxCollider2D>();
        }
        
        private void Start()
        {
            StartHandler();
        }
        
        private void Update()
        {
            if (!_isAttackable)
                return;
            Attack();
        }
        
        public void SpawnArrow()
        {
            var x = Instantiate(arrow, arrowTrans.position, Quaternion.identity);

           if (player == Players.Player2)
                x.transform.eulerAngles = new Vector3(0, 0, 180);
            x.Initialize(this, hero.damage, player);
        }
    }
}

