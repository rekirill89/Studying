using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace DuelGame
{
    public interface ILocalAssetLoader
    {
        public UniTask<T> LoadAsset<T>(AssetReference assetReference, CancellationToken token) where T : UnityEngine.Object;

        public UniTask<HeroStats> LoadHeroStats(AssetReference assetReference, CancellationToken token);

        public UniTask<BaseHero> LoadHero(AssetReference assetReference, CancellationToken token);
        
        public UniTask<GameObject> LoadBuffPrefab(AssetReference assetReference, CancellationToken token);
        
        public void UnloadAsset(AssetReference assetRef);
    }
}