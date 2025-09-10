using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class AudioAssetsLoader : IDisposable
    {
        public delegate void AudioAssetsLoaded(IAudioPlayer audioPlayer, Musics musics);
        public event AudioAssetsLoaded OnAssetsLoaded;

        public bool IsSystemReady { get; private set; } = false;
        
        private readonly AssetReference _soundEffectsRef;
        private readonly AssetReference _musicsRef;
        private readonly AssetReference _musicPlayerRef;
        
        private readonly ILocalAssetLoader _localAssetLoader;
        private readonly DiContainer _diContainer;
        
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        public AudioAssetsLoader(
            ILocalAssetLoader localAssetLoader, 
            DiContainer diContainer,
            AssetReference soundEffectsRef, 
            AssetReference musicsRef,
            AssetReference musicPlayerRef)
        {
            _localAssetLoader = localAssetLoader;
            _diContainer = diContainer;
            
            _soundEffectsRef = soundEffectsRef;
            _musicsRef = musicsRef;
            _musicPlayerRef = musicPlayerRef;
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
        
        public void Init()
        {
            LoadAssets().Forget();
        }

        private async UniTask LoadAssets()
        {
            try
            {
                var soundEffectsObj = await _localAssetLoader.LoadAsset<GameObject>(_soundEffectsRef, _cts.Token); // !
                var soundEffects = soundEffectsObj.GetComponent<SoundEffects>();
                await soundEffects.Init(_localAssetLoader, _cts.Token);
                _diContainer.Bind<SoundEffects>().FromInstance(soundEffects).AsSingle();

                var musicPlayerObj = Object.Instantiate(await _localAssetLoader.LoadAsset<GameObject>(_musicPlayerRef, _cts.Token)); // !
                var musicPlayer = musicPlayerObj.GetComponent<IAudioPlayer>();
                _diContainer.Bind<IAudioPlayer>().FromInstance(musicPlayer).AsSingle();

                var musicsObj = await _localAssetLoader.LoadAsset<GameObject>(_musicsRef, _cts.Token);
                var musics = musicsObj.GetComponent<Musics>();
                await musics.Init(_localAssetLoader, _cts.Token);
                _diContainer.Bind<Musics>().FromInstance(musics).AsSingle();

                OnAssetsLoaded?.Invoke(musicPlayer, musics);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading assets cancelled");
            }

            IsSystemReady = true;
        }
    }
}