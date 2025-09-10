using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;
using Zenject.SpaceFighter;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class BattleVFXController :  IDisposable
    {
        private readonly int _initSizeOfPool = 8;
        
        private readonly BattleManager _battleManager;
        private readonly BattleSceneAssetsLoader _battleSceneAssetsLoader;

        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
                
        private BaseHero _player1;
        private BaseHero _player2;
        
        private BloodParticle _bloodPrefab;
        private DeathEffect _deathEffectPrefab;
        
        private  ObjectPool<BloodParticle> _bloodPool;
        
        public BattleVFXController(BattleManager battleManager, BattleSceneAssetsLoader battleSceneAssetsLoader)
        {
            _battleManager = battleManager;
            _battleSceneAssetsLoader = battleSceneAssetsLoader;

        }

        public void Init()
        {
            _battleManager.OnPlayersSpawned += OnPlayerSpawnedHandler;
            _battleSceneAssetsLoader.OnBattleSceneAssetsReady += OnBattleAssetsReadyHandler;
        }

        private void OnBattleAssetsReadyHandler(BattleSettingsFacade _, BloodParticle bloodPrefab, DeathEffect deathEffectPrefab)
        {
            _bloodPrefab = bloodPrefab;
            _deathEffectPrefab = deathEffectPrefab;
            
            _bloodPool = new ObjectPool<BloodParticle>(_bloodPrefab, _initSizeOfPool);
        }

        public void Dispose()
        {
            _battleManager.OnPlayersSpawned -= OnPlayerSpawnedHandler;
            _battleSceneAssetsLoader.OnBattleSceneAssetsReady -= OnBattleAssetsReadyHandler;

            if (_player1 != null)
            {
                _player1.OnTakeDamage -= PlayTakeDamageEffect;
                _player1.OnDeath -= PlayDeathEffect;
            }

            if (_player2 != null)
            {
                _player1.OnTakeDamage -= PlayTakeDamageEffect;
                _player1.OnDeath -= PlayDeathEffect;
            }
            
            _cts.Cancel();
        }

        private void OnPlayerSpawnedHandler(BattleState battlestate)
        {
            _player1 = _battleManager.Player1();
            _player2 = _battleManager.Player2();
            
            _player1.OnTakeDamage += PlayTakeDamageEffect;
            _player1.OnDeath += PlayDeathEffect;
            
            _player2.OnTakeDamage += PlayTakeDamageEffect;
            _player2.OnDeath += PlayDeathEffect;
        }

        private void PlayDeathEffect(BaseHero hero, Players player)
        {
            var effect = Object.Instantiate(_deathEffectPrefab, hero.transform.position, Quaternion.identity);
            effect.Play();
        }

        private void PlayTakeDamageEffect(BaseHero hero, float _, float __, float ___, bool isPhysicalDamage)
        {
            if(!isPhysicalDamage)
                return;
            
            Debug.Log("Start blood effect");
            
            PlayTakeDamageEffectAsync(hero.transform.position).Forget();
        }

        private async UniTask PlayTakeDamageEffectAsync(Vector3 heroPosition)
        {
            var bloodParticle = _bloodPool.Get();
            bloodParticle.Init(heroPosition);

            bloodParticle.Ps.Play();
        
            await UniTask.Delay(1000);
        
            bloodParticle.Ps.Stop(true, ParticleSystemStopBehavior.StopEmitting);
            _bloodPool.ReturnToPool(bloodParticle);
        }
    }
}