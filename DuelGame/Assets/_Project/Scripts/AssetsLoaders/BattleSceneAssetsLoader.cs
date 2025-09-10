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
        public delegate void BattleSceneAssetsReady(BattleSettingsFacade facade, BloodParticle bloodPrefab, DeathEffect deathEffectPrefab);
        public event BattleSceneAssetsReady OnBattleSceneAssetsReady;
        
        private readonly ILocalAssetLoader _localAssetLoader;
        private readonly BattleSettingsFacade _facade;

        private readonly CancellationTokenSource _cts;

        private readonly AssetReference _bloodPrefabRef;
        private readonly AssetReference _deathEffectPrefabRef;

        public BattleSceneAssetsLoader(
            ILocalAssetLoader localAssetLoader, 
            BattleSettingsFacade facade, 
            AssetReference bloodPrefabRef, 
            AssetReference deathEffectPrefabRef)
        {
            Debug.Log("Battle asset loader started");
            _localAssetLoader = localAssetLoader;
            _facade = facade;
            
            _bloodPrefabRef = bloodPrefabRef;
            _deathEffectPrefabRef = deathEffectPrefabRef;
            
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

                var bloodPrefabObj = await _localAssetLoader.LoadAsset<GameObject>(_bloodPrefabRef, _cts.Token);
                var bloodPrefab = bloodPrefabObj.GetComponent<BloodParticle>();
                
                var deathEffectPrefabObj = await _localAssetLoader.LoadAsset<GameObject>(_deathEffectPrefabRef, _cts.Token);
                var deathEffectPrefab = deathEffectPrefabObj.GetComponent<DeathEffect>();
                
                OnBattleSceneAssetsReady?.Invoke(_facade, bloodPrefab, deathEffectPrefab);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading asset cancelled");
                throw;
            }
        }
    }
}