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
        
        public StartPanelView StartPanelView {get; private set;}
        public ContinuePanelView ContinuePanelView {get; private set;}
        public RestartPanelView RestartPanelView {get; private set;}
        public ReloadPanelView ReloadPanelView {get; private set;}
        public SavePanelView SavePanelView {get; private set;}
        public LoadPanelView LoadPanelView {get; private set;}
        public AdsPanelView AdsPanelView {get; private set;}
        
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
        }

        public void Init(ILocalAssetLoader localAssetLoader)
        {
            _localAssetLoader = localAssetLoader;
        }
        
        public async UniTask LoadAssets(CancellationToken token)
        {
            var startPanelViewObj = await _localAssetLoader.LoadAsset<GameObject>(StartPanelViewRef, token);
            var continuePanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(ContinuePanelViewRef, token);
            var restartPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(RestartPanelViewRef, token);
            var reloadPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(ReloadPanelViewRef, token);
            var savePanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(SavePanelViewRef, token);
            var loadPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(LoadPanelViewRef, token);
            var adsPanelViewObj = await _localAssetLoader.LoadAsset<GameObject>(AdsPanelViewRef, token);
            
            StartPanelView = startPanelViewObj.GetComponent<StartPanelView>();
            ContinuePanelView = continuePanelViewObj.GetComponent<ContinuePanelView>();
            RestartPanelView = restartPanelViewObj.GetComponent<RestartPanelView>();
            SavePanelView = savePanelViewObj.GetComponent<SavePanelView>();
            LoadPanelView = loadPanelViewObj.GetComponent<LoadPanelView>();
            ReloadPanelView = reloadPanelViewObj.GetComponent<ReloadPanelView>();
            AdsPanelView = adsPanelViewObj.GetComponent<AdsPanelView>();
            
            Debug.Log("Panels initialized");
        }
    }
}