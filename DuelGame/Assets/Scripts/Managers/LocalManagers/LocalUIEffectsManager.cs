using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class LocalUIEffectsManager : IBaseManager
    {
        private DamageText _damageText;
        private Transform _parentForTexts;
        private Transform _textPositionPl1;
        private Transform _textPositionPl2;
        private SpriteRenderer _iconPlayer1SP;
        private SpriteRenderer _iconPlayer2SP;

        private BaseHero _player1;
        private BaseHero _player2;

        public GameEventManager gameEventManager { get; set; }
        private CorutineManager _corutineManager;
        private ObjectPool<DamageText> _damageTextPool;

        public LocalUIEffectsManager(GameEventManager gameEventManager, CorutineManager corutineManager)
        {
            this.gameEventManager = gameEventManager;
            _corutineManager = corutineManager;

            gameEventManager.OnPlayersInstantiated += InitializePlayers;
            gameEventManager.OnBuffApplied += BuffAppliedHandler;
        }        
        private void InitializePlayers(BaseHero player1, BaseHero player2)
        {
            _player1 = player1;
            _player2 = player2;

            _player1.OnTakeHit += SpawnDamageText;
            _player2.OnTakeHit += SpawnDamageText;
        }

        public void SpawnDamageText(float damage, Players player)
        {
            Transform trans = player == Players.Player1 ? _textPositionPl1 : _textPositionPl2;

            var x =_damageTextPool.Get();
            x.Initialize(damage, _damageTextPool.ReturnToPool);

            x.transform.SetParent(trans);
            x.transform.localPosition = new Vector2(0, 0);
            x.transform.localScale = new Vector3(0.09f, 0.09f, 1);


        }

        public IEnumerator SpawnBuffIcon(Sprite sprite, float duration, Players player)
        {
            SpriteRenderer sp = default;
            if(player == Players.Player1)
            {
                sp = _iconPlayer1SP;
            }
            else if(player == Players.Player2)
            {
                sp = _iconPlayer2SP;
            }

            sp.enabled = true;
            sp.sprite = sprite;

            yield return new WaitForSeconds(duration);
            sp.enabled = false;
        }

        public void InitializeUIElementsNObjectPool(LocalUIEffectsFacade facade)
        {
            _damageText = facade.damageText;
            _parentForTexts = facade.parentForTexts;
            _textPositionPl1 = facade.textPositionPl1;
            _textPositionPl2 = facade.textPositionPl2;
            _iconPlayer1SP = facade.iconPlayer1SP;
            _iconPlayer2SP = facade.iconPlayer2SP;

            _damageTextPool = new ObjectPool<DamageText>(_damageText, 8, _parentForTexts);

            Debug.Log(_damageText);
        }

        private void BuffAppliedHandler(Sprite sprite, float damage, Players player)
        {
            _corutineManager.StartCoroutine(SpawnBuffIcon(sprite, damage, player));
        }
        
    }
}

