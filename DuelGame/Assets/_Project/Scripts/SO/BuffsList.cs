using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task Init(ILocalAssetLoader localAssetLoader)
        {
            _localAssetLoader = localAssetLoader;

            foreach (var buff in ListOfEntities)
            {
                var obj = await _localAssetLoader.LoadAsset<GameObject>(buff.SpReference);
                if (obj.TryGetComponent<SpriteRenderer>(out var spriteRenderer))
                    buff.Sp = spriteRenderer;
                else
                    Debug.LogError($"Incorrect asset {buff.SpReference}, no SpriteRenderer");
            }
        }
    }        

    [System.Serializable]
     public class EntryBuff : INamedObject
     {
        [FormerlySerializedAs("AssetSpReference")] public AssetReference SpReference;
        public BuffEnum BuffEnum;
        public SpriteRenderer Sp { get; set; }
     }

    public enum BuffEnum
    {
        Poison,
        DecreaseDamage,
        Stun,
        None
    }
}