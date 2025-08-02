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

        private void OnDestroy()
        {
            _localAssetLoader.UnloadAsset(BattleConfigRef);
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
                BattleConfig = await _localAssetLoader.LoadAsset<BattleConfig>(BattleConfigRef, token);
                Debug.Log("Battle config initialized");
            }
            catch (OperationCanceledException)
            {
                Debug.LogError("Loading asset cancelled");
                throw;
            }
        }
    }
}