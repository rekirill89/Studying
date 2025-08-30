using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DuelGame
{
    public class SkinAssetsLoader:IDisposable
    {
        public delegate void SkinsAocLoaded(SkinsList skinsList);
        public event SkinsAocLoaded OnSkinsAocLoaded;
        public bool IsSystemReady {get; private set;} = false;
        
        private readonly ILocalAssetLoader _localAssetLoader;
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        
        private readonly AssetReference _skinsRef;
        
        private SkinsList _skinsList;

        private Dictionary<HeroEnum, AnimatorOverrideController> _skinsAocDictionary;
        
        private CancellationTokenSource _cts = new CancellationTokenSource();

        public SkinAssetsLoader(
            ILocalAssetLoader localAssetLoader,
            GlobalAssetsLoader globalAssetsLoader,
            AssetReference skinsRef)
        {
            _localAssetLoader = localAssetLoader;
            _globalAssetsLoader = globalAssetsLoader;
            _skinsRef = skinsRef;
        }

        public void Dispose()
        {
            _cts.Cancel();
        }
        
        public void Init()
        {
            _globalAssetsLoader.OnDataLoaded += Initialize;
        }

        private void Initialize(GameLocalConfigs _, UILocalConfigs __)
        {
            LoadSkinsAsync().Forget();
        }

        private async UniTask LoadSkinsAsync()
        {
            _skinsList = await _localAssetLoader.LoadAsset<SkinsList>(_skinsRef, _cts.Token);
            _skinsList.Initialize(_localAssetLoader);

            OnSkinsAocLoaded?.Invoke(_skinsList);
            
            Debug.Log($"{this} is ready");
            IsSystemReady = true;
        }
    }
}