using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using IInitializable = Zenject.IInitializable;

namespace DuelGame
{
    public class BattleSceneAssetsLoader : IInitializable, IDisposable
    {
        public delegate void BattleSceneAssetsReady(BattleSettingsFacade facade, Panels panels);
        public event BattleSceneAssetsReady OnBattleSceneAssetsReady;
        //public event Action OnReadyToStart;
        
        private readonly ILocalAssetLoader _localAssetLoader;
        private readonly BattleSettingsFacade _facade;
        private readonly AssetReference _panelsRef;

        private readonly CancellationTokenSource _cts;

        public BattleSceneAssetsLoader(ILocalAssetLoader localAssetLoader, BattleSettingsFacade facade, AssetReference panelsRef)
        {
            Debug.Log("Battle asset loader started");
            _localAssetLoader = localAssetLoader;
            _facade = facade;
            _panelsRef = panelsRef;
            
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
            _facade.Init(_localAssetLoader);
            await _facade.LoadAssets(_cts.Token);

            var panelsObj = await _localAssetLoader.LoadAsset<GameObject>(_panelsRef, _cts.Token);
            var panels = panelsObj.GetComponent<Panels>();
            panels.Init(_localAssetLoader);
            await panels.LoadAssets(_cts.Token);
            
            OnBattleSceneAssetsReady?.Invoke(_facade, panels);
            //OnReadyToStart?.Invoke();
        }
    }
}