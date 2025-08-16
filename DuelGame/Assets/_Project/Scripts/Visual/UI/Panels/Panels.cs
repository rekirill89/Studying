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
        [SerializeField] public AssetReference StartPanelViewRef;
        [SerializeField] public AssetReference ContinuePanelViewRef;
        [SerializeField] public AssetReference RestartPanelViewRef;
        [SerializeField] public AssetReference ReloadPanelViewRef;
        [SerializeField] public AssetReference SavePanelViewRef;
        [SerializeField] public AssetReference LoadPanelViewRef;
        [SerializeField] public AssetReference AdsPanelViewRef;
        [SerializeField] public AssetReference MenuPanelViewRef;
        [SerializeField] public AssetReference SkinShopPanelViewRef;
        
        public StartPanelView StartPanelView {get; private set;}
        public ContinuePanelView ContinuePanelView {get; private set;}
        public RestartPanelView RestartPanelView {get; private set;}
        public ReloadPanelView ReloadPanelView {get; private set;}
        public SavePanelView SavePanelView {get; private set;}
        public LoadPanelView LoadPanelView {get; private set;}
        public AdsPanelView AdsPanelView {get; private set;}
        public MenuPanelView MenuPanelView {get; private set;}
        public SkinShopPanelView SkinShopPanelView {get; private set;}

        
        private ILocalAssetLoader _localAssetLoader;

        private void OnDestroy()
        {
            _localAssetLoader.UnloadAsset(StartPanelViewRef);
            _localAssetLoader.UnloadAsset(ContinuePanelViewRef);
            _localAssetLoader.UnloadAsset(RestartPanelViewRef);
            _localAssetLoader.UnloadAsset(ReloadPanelViewRef);
            _localAssetLoader.UnloadAsset(SavePanelViewRef);
            _localAssetLoader.UnloadAsset(LoadPanelViewRef);
            _localAssetLoader.UnloadAsset(AdsPanelViewRef);
            _localAssetLoader.UnloadAsset(MenuPanelViewRef);
            _localAssetLoader.UnloadAsset(SkinShopPanelViewRef);
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
                var startPanelViewObj = await _localAssetLoader.LoadAsset<GameObject>(StartPanelViewRef, token);
                var continuePanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(ContinuePanelViewRef, token);
                var restartPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(RestartPanelViewRef, token);
                var reloadPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(ReloadPanelViewRef, token);
                var savePanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(SavePanelViewRef, token);
                var loadPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(LoadPanelViewRef, token);
                var adsPanelViewObj = await _localAssetLoader.LoadAsset<GameObject>(AdsPanelViewRef, token);
                var menuPanelViewObj = await _localAssetLoader.LoadAsset<GameObject>(MenuPanelViewRef, token);
                var skinShopPanelObj = await _localAssetLoader.LoadAsset<GameObject>(SkinShopPanelViewRef, token);
            
                StartPanelView = startPanelViewObj.GetComponent<StartPanelView>();
                ContinuePanelView = continuePanelViewObj.GetComponent<ContinuePanelView>();
                RestartPanelView = restartPanelViewObj.GetComponent<RestartPanelView>();
                SavePanelView = savePanelViewObj.GetComponent<SavePanelView>();
                LoadPanelView = loadPanelViewObj.GetComponent<LoadPanelView>();
                ReloadPanelView = reloadPanelViewObj.GetComponent<ReloadPanelView>();
                AdsPanelView = adsPanelViewObj.GetComponent<AdsPanelView>();
                MenuPanelView = menuPanelViewObj.GetComponent<MenuPanelView>();
                SkinShopPanelView = skinShopPanelObj.GetComponent<SkinShopPanelView>();
            
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