using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "BuffsList", menuName = "Scriptable Objects/BuffsList")]
    public class BuffsList : EntityListScriptable<EntryBuff>
    {
        [field:SerializeField] public override List<EntryBuff> ListOfEntities { get; set; } = new List<EntryBuff>();
        
        private ILocalAssetLoader _localAssetLoader;

        public async UniTask Init(ILocalAssetLoader localAssetLoader, CancellationToken token)
        {
            _localAssetLoader = localAssetLoader;

            await LoadBuffs(token);
        }
        
        private async UniTask LoadBuffs(CancellationToken token)
        {
            foreach (var buff in ListOfEntities)
            {
                var buffObj = await _localAssetLoader.LoadBuffPrefab(buff.SpReference, token);
                if(buffObj.TryGetComponent<SpriteRenderer>(out var sp))
                    buff.Sp = sp;
                else
                    Debug.LogError($"Failed to load {buff.BuffEnum} sprite renderer");
            }
        }
    }        

    [System.Serializable]
     public class EntryBuff : INamedObject
     {
        [FormerlySerializedAs("AssetSpReference")] public AssetReference SpReference;
        public BuffEnum BuffEnum;
        public SpriteRenderer Sp { get; set; }
        public bool IsLoaded { get; set; } = false;
     }

    public enum BuffEnum
    {
        Poison,
        DecreaseDamage,
        Stun,
        None
    }
}