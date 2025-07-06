using System;
using System.Threading.Tasks;
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
        
        public StartPanelView StartPanelView {get; private set;}
        public ContinuePanelView ContinuePanelView {get; private set;}
        public RestartPanelView RestartPanelView {get; private set;}
        public ReloadPanelView ReloadPanelView {get; private set;}
        public SavePanelView SavePanelView {get; private set;}
        public LoadPanelView LoadPanelView {get; private set;}
        
        private ILocalAssetLoader _localAssetLoader;

        public void Init(ILocalAssetLoader localAssetLoader)
        {
            _localAssetLoader = localAssetLoader;
        }
        
        public async Task LoadAssets()
        {
            var startPanelViewObj = await _localAssetLoader.LoadAsset<GameObject>(StartPanelViewRef);
            var continuePanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(ContinuePanelViewRef);
            var restartPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(RestartPanelViewRef);
            var reloadPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(ReloadPanelViewRef);
            var savePanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(SavePanelViewRef);
            var loadPanelViewObj  = await _localAssetLoader.LoadAsset<GameObject>(LoadPanelViewRef);
            
            StartPanelView = startPanelViewObj.GetComponent<StartPanelView>();
            ContinuePanelView = continuePanelViewObj.GetComponent<ContinuePanelView>();
            RestartPanelView = restartPanelViewObj.GetComponent<RestartPanelView>();
            SavePanelView = savePanelViewObj.GetComponent<SavePanelView>();
            LoadPanelView = loadPanelViewObj.GetComponent<LoadPanelView>();
            ReloadPanelView = reloadPanelViewObj.GetComponent<ReloadPanelView>();
            
            Debug.Log("Panels initialized");
        }
    }
}