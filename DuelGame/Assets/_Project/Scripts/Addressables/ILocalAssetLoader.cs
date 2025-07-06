using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace DuelGame
{
    public interface ILocalAssetLoader
    {
        //public Task<T> LoadAsset<T>(string assetKey) where T : UnityEngine.Object;
        
        public Task<T> LoadAsset<T>(AssetReference assetReference) where T : UnityEngine.Object;

        public void UnloadAsset(AssetReference assetRef);
    }
}