using System;
using UnityEngine;

namespace DuelGame
{
    public class SkinSlotViewPresenter : IPresenter<SkinSlotView>
    {
        public event Action OnSkinBought;
        public SkinSlotView SkinSlotView {get;}
        
        private readonly SkinsController _skinsController;

        public SkinSlotViewPresenter(SkinsController skinsController, SkinSlotView skinSlotView)
        {
            _skinsController = skinsController;
            SkinSlotView = skinSlotView;
        }

        public void Initialize()
        {
            SkinSlotView.BuySkinButton.OnClick += OnClickBuyButtonHandler;
            SkinSlotView.EquipSkinButton.OnClick += OnEquipButtonClickHandler;
            SkinSlotView.UnEquipSkinButton.OnClick += OnUnEquipButtonClickHandler;
            
            _skinsController.OnSkinBought += OnSkinBoughtHandler;
            _skinsController.OnSkinEquipped += OnSkinEquippedHandler;
            _skinsController.OnSkinDataCleared += OnSkinDataClearedHandler;

            if (CheckSkinBuyStatus())
                ChangeBuyEquipUnEquipButtonStatus(false, true, false);
        }

        public void Dispose()
        {
            SkinSlotView.BuySkinButton.OnClick -= OnClickBuyButtonHandler;
            SkinSlotView.EquipSkinButton.OnClick -= OnEquipButtonClickHandler;
            SkinSlotView.UnEquipSkinButton.OnClick -= OnUnEquipButtonClickHandler;
            
            _skinsController.OnSkinBought -= OnSkinBoughtHandler;
            _skinsController.OnSkinEquipped -= OnSkinEquippedHandler;
            _skinsController.OnSkinDataCleared -= OnSkinDataClearedHandler;
        }

        public void ShowView()
        {
            SkinSlotView.Show();
        }

        public void HideView()
        {
            SkinSlotView.Hide();
        }
        
        private void OnSkinEquippedHandler(SkinEnum skinEnum)
        {
            if (CheckSkinBuyStatus())
            {
                if (SkinSlotView.SkinEnum == skinEnum)
                    ChangeBuyEquipUnEquipButtonStatus(false, false, true);
                else
                    ChangeBuyEquipUnEquipButtonStatus(false, true, false);
            }
        }
        
        private bool CheckSkinBuyStatus()
        {
            if (_skinsController.BoughtSkins.Contains(SkinSlotView.SkinEnum))
                return true;
            
            return false;
        }
        
        private void ChangeBuyEquipUnEquipButtonStatus(bool buyButton, bool equipButton, bool unEquipButton)
        {
            SkinSlotView.BuySkinButton.SetInteractable(buyButton);
            SkinSlotView.EquipSkinButton.SetInteractable(equipButton);
            SkinSlotView.UnEquipSkinButton.SetInteractable(unEquipButton);
        }

        private void OnSkinDataClearedHandler()
        {
            ChangeBuyEquipUnEquipButtonStatus(true, false, false);
        }
        
        private void OnEquipButtonClickHandler()
        {
            _skinsController.SetSkinToHero(SkinSlotView.HeroEnum, SkinSlotView.SkinEnum);
        }
        
        private void OnUnEquipButtonClickHandler()
        {
            _skinsController.SetDefaultSkinToHero(SkinSlotView.HeroEnum);
        }
        
        private void OnClickBuyButtonHandler()
        {
            _skinsController.BuySkin(SkinSlotView.SkinEnum);
        }
        
        private void OnSkinBoughtHandler(SkinEnum skinEnum)
        {
            if (CheckSkinBuyStatus())
            {
                if (SkinSlotView.SkinEnum == skinEnum)
                    ChangeBuyEquipUnEquipButtonStatus(false, true, false);
                
                OnSkinBought?.Invoke();
            }
        }
    }
}