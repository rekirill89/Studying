using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
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

        public async UniTask LoadAssets(CancellationToken token)
        {
            BattleConfig = await _localAssetLoader.LoadAsset<BattleConfig>(BattleConfigRef, token);
            Debug.Log("Battle config initialized");
        }
    }
}