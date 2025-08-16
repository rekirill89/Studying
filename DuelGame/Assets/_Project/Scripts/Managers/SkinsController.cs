using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class SkinsController : IDisposable
    {
        public delegate void SkinBought(SkinEnum skinEnum);
        public delegate void SkinEquipped(SkinEnum skinEnum);
        public event SkinBought OnSkinBought;
        public event SkinEquipped OnSkinEquipped;
        public event Action OnSkinDataCleared;

        public List<SkinEnum> BoughtSkins { get; private set; }

        private readonly SkinAssetsLoader _skinAssetsLoader;
        private readonly GlobalAssetsLoader _globalAssetsLoader;
        private readonly SaveService _saveService;
        
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();

        private readonly Dictionary<SkinEnum, HeroEnum> _boughtSkinHeroeEnum;

        private readonly bool _isSkinDataClearMode = true;

        private HeroesList _heroesList;
        private SkinsList _skinsList;
        
        public SkinsController(
            SkinAssetsLoader skinAssetsLoader, 
            GlobalAssetsLoader globalAssetsLoader,
            SaveService saveService)
        {
            _skinAssetsLoader = skinAssetsLoader;
            _globalAssetsLoader = globalAssetsLoader;
            _saveService = saveService;

            BoughtSkins = new List<SkinEnum>();
            
            _boughtSkinHeroeEnum = new Dictionary<SkinEnum, HeroEnum>()
            {
                { SkinEnum.WarriorDefault, HeroEnum.Warrior },
                { SkinEnum.WarriorUniq , HeroEnum.Warrior},
                { SkinEnum.ArcherDefault, HeroEnum.Archer },
                { SkinEnum.ArcherUniq , HeroEnum.Archer},
                { SkinEnum.WizardDefault , HeroEnum.Wizard},
                { SkinEnum.WizardUniq , HeroEnum.Wizard}
            };
        }
        
        public void Dispose()
        {
            _globalAssetsLoader.OnDataLoaded -= GlobalAssetsLoaderOnOnDataLoaded;
            _skinAssetsLoader.OnSkinsAocLoaded -= OnSkinsAocLoadedHandler;
            _saveService.OnSkinSaveCleared -= OnSkinDataClearedHandler;
        }
        
        public void Init()
        {
            _globalAssetsLoader.OnDataLoaded += GlobalAssetsLoaderOnOnDataLoaded;
            _skinAssetsLoader.OnSkinsAocLoaded += OnSkinsAocLoadedHandler;
            _saveService.OnSkinSaveCleared += OnSkinDataClearedHandler;
        }
        
        public void BuySkin(SkinEnum skinEnum)
        {
            var x = _saveService.LoadSkinsData() ?? new SkinsData();
            
            x.AddBoughtSkin(skinEnum);
            BoughtSkins = x.BoughtSkins;
            _saveService.SaveSkinsData(x);
            
            Debug.Log($"Skin {skinEnum} has been bought");
            OnSkinBought?.Invoke(skinEnum);
        }
        
        public void SetSkinToHero(HeroEnum heroEnum, SkinEnum skinEnum)
        {
            SetSkinToHeroAsync(heroEnum, skinEnum).Forget();
        }

        public void SetDefaultSkinToHero(HeroEnum heroEnum)
        {
            SetSkinToHeroAsync(
                heroEnum, 
                _boughtSkinHeroeEnum.First(x => x.Value == heroEnum).Key
            ).Forget();
        }
        
        private void OnSkinDataClearedHandler()
        {
            BoughtSkins.Clear();
            OnSkinDataCleared?.Invoke();
        }
        
        private void GlobalAssetsLoaderOnOnDataLoaded(GameLocalConfigs localconfigs, UILocalConfigs _)
        {
            _heroesList = localconfigs.HeroesList;
        }

        private void OnSkinsAocLoadedHandler(SkinsList skinsList)
        {
            _skinsList = skinsList;

            OnSkinsAocLoadedHandlerAsync().Forget();
        }
        
        private async UniTask OnSkinsAocLoadedHandlerAsync()
        {
            await SetSkinToHeroAsync(HeroEnum.Archer, SkinEnum.ArcherDefault);
            await SetSkinToHeroAsync(HeroEnum.Warrior, SkinEnum.WarriorDefault);
            await SetSkinToHeroAsync(HeroEnum.Wizard, SkinEnum.WizardDefault);

            if (_isSkinDataClearMode)
                _saveService.ClearSkinsData();

            var data = _saveService.LoadSkinsData();
            if (data != null)
                BoughtSkins = data.BoughtSkins;
        }

        private async UniTask SetSkinToHeroAsync(HeroEnum heroEnum, SkinEnum skinEnum)
        {
            var aoc = await _skinsList.LoadSkinAsync(skinEnum, _cts.Token);
            
            _heroesList.ListOfEntities
                .First(x => x.HeroEnum == heroEnum)
                .Aoc = aoc;
            
            OnSkinEquipped?.Invoke(skinEnum);
            Debug.Log($"Skin {skinEnum} has been equiped");
        }
    }

    [Serializable]
    public class SkinsData
    {
        [field:SerializeField]public List<SkinEnum> BoughtSkins { get; private set; }

        public SkinsData()
        {
            BoughtSkins = new List<SkinEnum>();
        }

        public void AddBoughtSkin(SkinEnum skinEnum)
        {
            if(BoughtSkins.Contains(skinEnum))
                return;
            
            BoughtSkins.Add(skinEnum);
            Debug.Log(BoughtSkins.Count);
        }
    }
}