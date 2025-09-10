using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Animations;
using Zenject;

namespace DuelGame
{
    public class BackgroundMusicController : IDisposable
    {
        private readonly AudioAssetsLoader _audioAssetsLoader;
        private readonly SceneState _sceneState;
        
        private readonly Dictionary<SceneEnum, Action> _sceneActions;

        private IAudioPlayer _audioPlayer;
        private AudioClip _menuMusic;
        private AudioClip _battleMusic;
        
        private CancellationTokenSource _cts = new CancellationTokenSource();
        
        public BackgroundMusicController(AudioAssetsLoader audioAssetsLoader, SceneState sceneState)
        {
            _audioAssetsLoader = audioAssetsLoader;
            _sceneState = sceneState;

            _sceneActions = new Dictionary<SceneEnum, Action>()
            {
                { SceneEnum.BattleScene, () =>  PlayBattleMusicAsync().Forget() },
                { SceneEnum.MenuScene, () => PlayMenuMusicAsync().Forget() },
            };
        }
        
        public void Dispose()
        {
            _audioAssetsLoader.OnAssetsLoaded -= OnAssetsLoadedHandler;
            
            _cts.Cancel();
        }
        
        public void Init()
        {
            _audioAssetsLoader.OnAssetsLoaded += OnAssetsLoadedHandler;
            _sceneState.OnSceneChanged += OnSceneChangedHandler;
        }

        private void OnSceneChangedHandler()
        {
            Debug.Log($"Scene changed from {_sceneState.PreviousScene} to {_sceneState.CurrentScene}");
            if (_sceneState.CurrentScene != _sceneState.PreviousScene)
            {
                Debug.Log($"Not the same scene");
                _cts.Cancel();
                _cts = new CancellationTokenSource();
                _sceneActions[_sceneState.CurrentScene]?.Invoke();
            }
        }

        private async UniTask PlayMenuMusicAsync()
        {
            await UniTask.WaitUntil(() => _audioAssetsLoader.IsSystemReady, cancellationToken:_cts.Token);
            
            _audioPlayer.Play(_menuMusic);
        }

        private async UniTask PlayBattleMusicAsync()
        {
            await UniTask.WaitUntil(() => _audioAssetsLoader.IsSystemReady, cancellationToken:_cts.Token);

            _audioPlayer.Play(_battleMusic);
        }

        private void OnAssetsLoadedHandler(IAudioPlayer audioPlayer, Musics musics)
        {
            _audioPlayer = audioPlayer;
            _menuMusic = musics.MenuMusic;
            _battleMusic = musics.BattleMusic;
        }
    }
}