using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DuelGame;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace DuelGame
{
    public class Musics : MonoBehaviour, IDisposable
    {
        [SerializeField] private AssetReference _menuMusicRef;
        [SerializeField] private AssetReference _battleMusicRef;
        //[SerializeField] private AssetReference _hitSERef;

        public AudioClip MenuMusic { get; private set; }
        public AudioClip BattleMusic { get; private set; }
        //public AudioClip HitSE { get; private set; }

        private ILocalAssetLoader _assetLoader;

        public void DoSomething()
        {
            ;}
        public void Dispose()
        {
            _assetLoader.UnloadAsset(_menuMusicRef);
            _assetLoader.UnloadAsset(_battleMusicRef);
            //_assetLoader.UnloadAsset(_hitSERef);
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
                MenuMusic = await _assetLoader.LoadAsset<AudioClip>(_menuMusicRef, token);
                BattleMusic = await _assetLoader.LoadAsset<AudioClip>(_battleMusicRef, token);
                //HitSE = await _assetLoader.LoadAsset<AudioClip>(_hitSERef, token);
            }
            catch (OperationCanceledException)
            {
                Debug.LogWarning("Musics loading canceled");
            }
        }
    }
}

