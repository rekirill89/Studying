using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DuelGame
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Archer : BaseHero
    {        
        [SerializeField] private Transform _arrowTrans;
        [SerializeField] private Arrow _arrow;
        
        private const float ANGLE = 180;
        private void Awake()
        {
            _bodyCollider = GetComponent<BoxCollider2D>();
        }
        
        public void SpawnArrow()
        {
            var x = Instantiate(_arrow, _arrowTrans.position, Quaternion.identity);
            
            
            if (_player == Players.Player2)
                x.transform.eulerAngles = new Vector3(0, 0, ANGLE);
           
            x.Initialize(
                hero.damage, 
                _player,
                InvokeApplyBuffToEnemy);
        }
    }
}

