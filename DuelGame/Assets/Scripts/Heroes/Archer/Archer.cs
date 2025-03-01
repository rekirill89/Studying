using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class Archer : BaseHero
    {        
        [field: SerializeField] public override Image healthBar { get; set; }        
        [SerializeField] private Transform _arrowTrans;
        [SerializeField] private Arrow _arrow;

        private void Awake()
        {
            _bodyCollider = GetComponent<BoxCollider2D>();
        }
        private void Start()
        {
            StartHandler();
        }
        private void FixedUpdate()
        {
            if (!_isAttackable) return;
            Attack();
        }
        public void SpawnArrow()
        {
            var x = Instantiate(_arrow, _arrowTrans.position, Quaternion.identity);

           if (player == Players.Player2)
                x.transform.eulerAngles = new Vector3(0, 0, 180);
            x.Initialize(this, hero.damage, player);
        }
    }
}

