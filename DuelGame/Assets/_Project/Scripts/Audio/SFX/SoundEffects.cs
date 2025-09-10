using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DuelGame
{
    public class SoundEffects : MonoBehaviour, IDisposable
    {
        [SerializeField] private AssetReference _deathSERef;
        [SerializeField] private AssetReference _attackSERef;
        [SerializeField] private AssetReference _hitSERef;
        [SerializeField] private AssetReference _youDiedSERef;
        [SerializeField] private AssetReference _enemyFelledSERef;

        public AudioClip DeathSE { get; private set; }
        public AudioClip AttackSE { get; private set; }
        public AudioClip HitSE { get; private set; }
        public AudioClip YouDiedSE { get; private set; }
        public AudioClip EnemyFelledSE { get; private set; }
        

        private ILocalAssetLoader _assetLoader;
        
        public void Dispose()
        {
            _assetLoader.UnloadAsset(_deathSERef);
            _assetLoader.UnloadAsset(_attackSERef);
            _assetLoader.UnloadAsset(_hitSERef);
            _assetLoader.UnloadAsset(_youDiedSERef);
            _assetLoader.UnloadAsset(_enemyFelledSERef);
        }
        
        public async UniTask Init(ILocalAssetLoader assetLoader, CancellationToken token)
        {
            _assetLoader = assetLoader;

            await LoadAssets(token);
        }

        private async UniTask LoadAssets(CancellationToken token)
        {
            try
            {
                DeathSE = await _assetLoader.LoadAsset<AudioClip>(_deathSERef, token);
                AttackSE = await _assetLoader.LoadAsset<AudioClip>(_attackSERef, token);
                HitSE = await _assetLoader.LoadAsset<AudioClip>(_hitSERef, token);
                YouDiedSE = await _assetLoader.LoadAsset<AudioClip>(_youDiedSERef, token);
                EnemyFelledSE = await _assetLoader.LoadAsset<AudioClip>(_enemyFelledSERef, token);

            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("SoundEffects loading canceled");
            }
        }
    }
}