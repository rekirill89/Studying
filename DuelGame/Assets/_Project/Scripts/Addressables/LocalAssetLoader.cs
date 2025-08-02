using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class LocalAssetLoader : ILocalAssetLoader
    {
        private readonly Dictionary<AssetReference, AsyncOperationHandle> _loadedAssets = new();
        
        public async UniTask<T> LoadAsset<T>(AssetReference assetReference, CancellationToken token) where T : Object
        {
            if (_loadedAssets.TryGetValue(assetReference, out var handle))
            {
                return (T)handle.Result;
            }
            
            var assetHandle = Addressables.LoadAssetAsync<T>(assetReference);
            await assetHandle.ToUniTask(cancellationToken:token);
            
            if (assetHandle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Error loading asset");
                return null;
            }

            _loadedAssets.Add(assetReference, assetHandle);

            return (T)assetHandle.Result;
        }

        public async UniTask<HeroStats> LoadHeroStats(AssetReference assetReference, CancellationToken token)
        {
            try
            {
                return await LoadAsset<HeroStats>(assetReference, token);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading asset cancelled");
                throw;
            }
        }

        public async UniTask<BaseHero> LoadHero(AssetReference assetReference, CancellationToken token)
        {
            try
            {
                var x = await LoadAsset<GameObject>(assetReference, token);
                return x.GetComponent<BaseHero>();
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading asset cancelled");
                throw;
            }
        }

        public async UniTask<GameObject> LoadBuffPrefab(AssetReference assetReference, CancellationToken token)
        {
            try
            {
                return await LoadAsset<GameObject>(assetReference, token);
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading asset cancelled");
                throw;
            }
        }

        public void UnloadAsset(AssetReference assetRef)
        {
            if (_loadedAssets.TryGetValue(assetRef, out var handle))
            {
                Addressables.Release(handle);
                _loadedAssets.Remove(assetRef);
            }
        }
    }
}
