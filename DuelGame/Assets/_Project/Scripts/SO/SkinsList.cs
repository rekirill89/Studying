using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Serialization;

namespace DuelGame
{
    [CreateAssetMenu(fileName = "SkinsList", menuName = "Scriptable Objects/SkinsList")]
    public class SkinsList : ScriptableObject
    {
        private ILocalAssetLoader _localAssetLoader;
        
        [FormerlySerializedAs("skins")] public List<SkinEntry> Skins = new List<SkinEntry>();

        public void Initialize(ILocalAssetLoader localAssetLoader)
        {
            _localAssetLoader = localAssetLoader;
        }

        public async UniTask<AnimatorOverrideController> LoadSkinAsync(SkinEnum skinEnum, CancellationToken token)
        {
            var skin = Skins.First(x => x.SkinEnum == skinEnum);

            var aoc = await _localAssetLoader.LoadAsset<AnimatorOverrideController>(skin.AocRef, token);
            return aoc;
        }
    }

    [System.Serializable]
    public class SkinEntry
    {
        //public AnimatorOverrideController Aoc { get; set; }
        public AssetReference AocRef;
        public SkinEnum SkinEnum;
    }

    public enum SkinEnum
    {
        WarriorDefault,
        ArcherDefault,
        WizardDefault,
        WarriorUniq,
        ArcherUniq,
        WizardUniq,
    }
}