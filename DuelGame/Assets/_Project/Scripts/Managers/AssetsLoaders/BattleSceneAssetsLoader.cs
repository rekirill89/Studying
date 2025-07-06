using System;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using IInitializable = Zenject.IInitializable;

namespace DuelGame
{
    public class BattleSceneAssetsLoader : IInitializable
    {
        public event Action<BattleSettingsFacade, Panels> OnBattleSceneAssetsPrepared;
        public event Action OnReadyToStart;
        
        private readonly ILocalAssetLoader _localAssetLoader;
        private readonly BattleSettingsFacade _facade;
        private readonly AssetReference _panelsRef;

        public BattleSceneAssetsLoader(ILocalAssetLoader localAssetLoader, BattleSettingsFacade facade, AssetReference panelsRef)
        {
            _localAssetLoader = localAssetLoader;
            _facade = facade;
            _panelsRef = panelsRef;
        }
        
        public void Initialize()
        {
            _ = Load();
        }
        
        private async Task Load()
        {
            _facade.Init(_localAssetLoader);
            await _facade.LoadAssets();

            var panelsObj = await _localAssetLoader.LoadAsset<GameObject>(_panelsRef);
            var panels = panelsObj.GetComponent<Panels>();
            panels.Init(_localAssetLoader);
            await panels.LoadAssets();
            
            OnBattleSceneAssetsPrepared?.Invoke(_facade, panels);
            OnReadyToStart?.Invoke();
        }
    }
}