using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;
using Zenject;

namespace DuelGame
{
    public class BattleSettingsFacade : MonoBehaviour
    {
        public BattleConfig BattleConfig { get; private set; }
        
        public AssetReference BattleConfigRef;
        public Transform FirstPlayerTrans;
        public Transform SecondPlayerTrans;
        
        private ILocalAssetLoader _localAssetLoader;
        
        public void Init(ILocalAssetLoader localAssetLoader)
        {
            _localAssetLoader = localAssetLoader;
        }

        public async Task LoadAssets()
        {
            BattleConfig = await _localAssetLoader.LoadAsset<BattleConfig>(BattleConfigRef);
            Debug.Log("Battle config initialized");
        }
    }
}