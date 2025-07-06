using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace DuelGame
{
    public class LocalAssetLoader : ILocalAssetLoader
    {
        private readonly Dictionary<AssetReference, AsyncOperationHandle> _loadedAssets = new();
        
        public async Task<T> LoadAsset<T>(AssetReference assetReference) where T : Object
        {
            if (_loadedAssets.TryGetValue(assetReference, out var handle))
            {
                return (T)handle.Result;
            }
            
            var assetHandle = Addressables.LoadAssetAsync<T>(assetReference);
            await assetHandle.Task;

            if (assetHandle.Status == AsyncOperationStatus.Failed)
            {
                Debug.LogError("Error loading asset");
                return null;
            }

            _loadedAssets.Add(assetReference, assetHandle);

            return (T)assetHandle.Result;
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
