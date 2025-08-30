using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DuelGame
{
    public class RemoteConfigsLoader : IDisposable, IRemoteConfigsLoader
    {
        public event RemoteConfigsApplied OnRemoteConfigsApplied;

        public bool IsSystemReady { get; private set; } = false;
        
        private const string REMOTE_CONFIG_KEY = "GameConfig";
        
        private readonly CancellationTokenSource _cts;
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        private readonly AssetReference _battleConfigRef;
        private readonly ILocalAssetLoader _localAssetLoader;
        
        private GameLocalConfigs _gameLocalConfigs;
        
        private bool _isInitialized;

        public RemoteConfigsLoader(
            GlobalAssetsLoader globalAssetsLoader, 
            AssetReference battleConfigRef,
            ILocalAssetLoader localAssetLoader
            )
        {
            _globalAssetsLoader = globalAssetsLoader;
            _battleConfigRef = battleConfigRef;
            _localAssetLoader = localAssetLoader;
            
            _cts = new CancellationTokenSource();
        }
        
        public void Dispose()
        {
            _globalAssetsLoader.OnDataLoaded -= OnDataLoadedHandler ;
        }
        
        public void Init()
        {
            _globalAssetsLoader.OnDataLoaded += OnDataLoadedHandler ;
        }

        private void OnDataLoadedHandler(GameLocalConfigs gameLocalConfigs, UILocalConfigs _)
        {
            _gameLocalConfigs = gameLocalConfigs;
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(System.TimeSpan.Zero)
                .ContinueWithOnMainThread(fetchTask =>
                {
                    if (fetchTask.IsCompleted)
                    {
                        FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                            .ContinueWithOnMainThread(activateTask =>
                            {
                                FetchRemoteConfigs().Forget();
                            });
                    }
                });
        }

        private async UniTask FetchRemoteConfigs()
        {
            string json = FirebaseRemoteConfig.DefaultInstance.GetValue(REMOTE_CONFIG_KEY).StringValue;
            var gameRemoteConfig = JsonUtility.FromJson<GameConfig>(json);

            await ApplyRemoteConfigs(gameRemoteConfig);
        }

        private async UniTask ApplyRemoteConfigs(GameConfig gameConfig)
        {
            _gameLocalConfigs.HeroesList
                .ListOfEntities
                .First(hero => hero.HeroEnum == HeroEnum.Archer)
                .HeroStats.UpdateStats(gameConfig.Heroes.Archer);
            _gameLocalConfigs.HeroesList
                .ListOfEntities
                .First(hero => hero.HeroEnum == HeroEnum.Warrior)
                .HeroStats.UpdateStats(gameConfig.Heroes.Warrior);
            _gameLocalConfigs.HeroesList
                .ListOfEntities
                .First(hero => hero.HeroEnum == HeroEnum.Wizard)
                .HeroStats.UpdateStats(gameConfig.Heroes.Wizard);

            try
            {
                var battleConfig = await _localAssetLoader.LoadAsset<BattleConfig>(_battleConfigRef, _cts.Token);
                battleConfig.SetConfigStats(gameConfig.Battle);
                _localAssetLoader.UnloadAsset(_battleConfigRef);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading asset cancelled");
                throw;
            }
            
            OnRemoteConfigsApplied?.Invoke(_gameLocalConfigs, gameConfig.Buffs);
            Debug.Log("RemoteConfigs applied successfully");
            
            Debug.Log($"{this} is ready");
            IsSystemReady = true;
        }
    }
}