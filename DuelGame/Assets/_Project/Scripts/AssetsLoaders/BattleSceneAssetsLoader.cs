using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;

namespace DuelGame
{
    public class BattleSceneAssetsLoader : IDisposable
    {
        public delegate void BattleSceneAssetsReady(BattleSettingsFacade facade);
        public event BattleSceneAssetsReady OnBattleSceneAssetsReady;
        
        private readonly ILocalAssetLoader _localAssetLoader;
        private readonly BattleSettingsFacade _facade;

        private readonly CancellationTokenSource _cts;

        public BattleSceneAssetsLoader(ILocalAssetLoader localAssetLoader, BattleSettingsFacade facade)
        {
            Debug.Log("Battle asset loader started");
            _localAssetLoader = localAssetLoader;
            _facade = facade;
            
            _cts = new CancellationTokenSource();
        }
        
        public void Initialize()
        {
            Debug.Log("Loading assets...");
            Load().Forget();
        }
        
        public void Dispose()
        {
            _cts.Cancel();
        }
        
        private async UniTask Load()
        {
            try
            {
                await _facade.Init(_localAssetLoader, _cts.Token);

                OnBattleSceneAssetsReady?.Invoke(_facade);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading asset cancelled");
                throw;
            }
        }
    }
}