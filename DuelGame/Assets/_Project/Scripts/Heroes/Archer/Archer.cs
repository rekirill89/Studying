using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
        private readonly List<Arrow> _arrows = new List<Arrow>();
        
        private void Awake()
        {
            BodyCollider = GetComponent<BoxCollider2D>();
        }
        
        private void OnDestroy()
        {
            foreach (var arrow in _arrows)
                arrow.OnArrowHit -= ArrowHitHandler;
        }
        
        public void SpawnArrow()
        {
            var x = Instantiate(_arrow, _arrowTrans.position, Quaternion.identity);
            
            _arrows.Add(x);
            
            if (Player == Players.Player2)
                x.transform.eulerAngles = new Vector3(0, 0, ANGLE);

            x.OnArrowHit += ArrowHitHandler;
            x.Initialize(Player);
        }

        private void ArrowHitHandler(Arrow arrow)
        {
            DamageEnemy();
            arrow.OnArrowHit -= ArrowHitHandler;
            _arrows.Remove(arrow);
        }
    }
}