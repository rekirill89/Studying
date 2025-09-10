using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class Panels : MonoBehaviour
    {        
        [SerializeField] private AssetReference _startPanelViewRef;
        [SerializeField] private AssetReference _continuePanelViewRef;
        [SerializeField] private AssetReference _restartPanelViewRef;
        [SerializeField] private AssetReference _reloadPanelViewRef;
        [SerializeField] private AssetReference _savePanelViewRef;
        [SerializeField] private AssetReference _loadPanelViewRef;
        [SerializeField] private AssetReference _adsPanelViewRef;
        [SerializeField] private AssetReference _menuPanelViewRef;
        [SerializeField] private AssetReference _skinShopPanelViewRef;
        [SerializeField] private AssetReference _battleFinishPanelViewRef;
        
        //[SerializeField] private AssetReference _youDiedPanelRef; 
        //[SerializeField] private AssetReference _enemyFelledPanelRef; 
        
        public StartView StartView {get; private set;}
        public ContinueView ContinueView {get; private set;}
        public RestartView RestartView {get; private set;}
        public ReloadView ReloadView {get; private set;}
        public SaveView SaveView {get; private set;}
        public LoadView LoadView {get; private set;}
        public AdsView AdsView {get; private set;}
        public MenuView MenuView {get; private set;}
        public SkinShopView SkinShopView { get; private set; }
        public BattleFinishView BattleFinishView { get; private set; } 
        
        //public YouDiedView YouDiedView { get; private set; }
        //public EnemyFelledView EnemyFelledView { get; private set; }
        
        private ILocalAssetLoader _localAssetLoader;

        private void OnDestroy()
        {
            _localAssetLoader.UnloadAsset(_startPanelViewRef);
            _localAssetLoader.UnloadAsset(_continuePanelViewRef);
            _localAssetLoader.UnloadAsset(_restartPanelViewRef);
            _localAssetLoader.UnloadAsset(_reloadPanelViewRef);
            _localAssetLoader.UnloadAsset(_savePanelViewRef);
            _localAssetLoader.UnloadAsset(_loadPanelViewRef);
            _localAssetLoader.UnloadAsset(_adsPanelViewRef);
            _localAssetLoader.UnloadAsset(_menuPanelViewRef);
            _localAssetLoader.UnloadAsset(_skinShopPanelViewRef);
            _localAssetLoader.UnloadAsset(_battleFinishPanelViewRef);
            
            //_localAssetLoader.UnloadAsset(_youDiedPanelRef);
            //_localAssetLoader.UnloadAsset(_enemyFelledPanelRef);
        }

        public async UniTask Init(ILocalAssetLoader localAssetLoader, CancellationToken token)
        {
            _localAssetLoader = localAssetLoader;

            await LoadAssets(token);
        }
        
        private async UniTask LoadAssets(CancellationToken token)
        {
            try
            {
                var startPanelViewObj = await _localAssetLoader.LoadAsset<GameObject>(_startPanelViewRef, token);
                var continuePanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(_continuePanelViewRef, token);
                var restartPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(_restartPanelViewRef, token);
                var reloadPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(_reloadPanelViewRef, token);
                var savePanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(_savePanelViewRef, token);
                var loadPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(_loadPanelViewRef, token);
                var adsPanelViewObj = await _localAssetLoader.LoadAsset<GameObject>(_adsPanelViewRef, token);
                var menuPanelViewObj = await _localAssetLoader.LoadAsset<GameObject>(_menuPanelViewRef, token);
                var skinShopPanelObj = await _localAssetLoader.LoadAsset<GameObject>(_skinShopPanelViewRef, token);
                var battleFinishPanelViewObj = await _localAssetLoader.LoadAsset<GameObject>(_battleFinishPanelViewRef, token);
                
                //var youDiedViewObj = await _localAssetLoader.LoadAsset<GameObject>(_youDiedPanelRef, token);
                //var enemyFelledViewObj = await _localAssetLoader.LoadAsset<GameObject>(_enemyFelledPanelRef, token);
            
                StartView = startPanelViewObj.GetComponent<StartView>();
                ContinueView = continuePanelViewObj.GetComponent<ContinueView>();
                RestartView = restartPanelViewObj.GetComponent<RestartView>();
                SaveView = savePanelViewObj.GetComponent<SaveView>();
                LoadView = loadPanelViewObj.GetComponent<LoadView>();
                ReloadView = reloadPanelViewObj.GetComponent<ReloadView>();
                AdsView = adsPanelViewObj.GetComponent<AdsView>();
                MenuView = menuPanelViewObj.GetComponent<MenuView>();
                SkinShopView = skinShopPanelObj.GetComponent<SkinShopView>();
                BattleFinishView = battleFinishPanelViewObj.GetComponent<BattleFinishView>();
                
                //YouDiedView = youDiedViewObj.GetComponent<YouDiedView>();
                //EnemyFelledView = enemyFelledViewObj.GetComponent<EnemyFelledView>();
            
                Debug.Log("Panels initialized");
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading asset cancelled");
                throw;
            }
        }
    }
}