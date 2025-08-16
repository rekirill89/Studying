using TMPro;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class MenuPanelPresenter : IPresenter<MenuPanelView>
    {
        private readonly MenuSceneEntryPoint _menuSceneEntryPoint;
        private readonly IInAppPurchaseService _inAppPurchaseService;
        private readonly IInstantiator _instantiator;
        private readonly DataCache _dataCache;
        private readonly MenuPanelView _menuPanelView;
        private readonly UIFactory _uiFactory;
        private readonly SaveService _saveService;
        private readonly SkinsController _skinsController;

        private SkinShopPanelPresenter _skinShopPanelPresenter;
        private SkinSlotViewPresenter _skinSlotViewPresenter;
        
        public MenuPanelPresenter(
            MenuSceneEntryPoint menuSceneEntryPoint, 
            IInAppPurchaseService inAppPurchaseService, 
            DataCache dataCache,
            UIFactory uiFactory,
            IInstantiator instantiator,
            SaveService saveService,
            SkinsController skinsController,
            MenuPanelView menuPanelView)
        {
            _menuSceneEntryPoint = menuSceneEntryPoint;
            _inAppPurchaseService = inAppPurchaseService;
            _dataCache = dataCache;
            _uiFactory = uiFactory;
            _instantiator = instantiator;
            _saveService = saveService;
            _menuPanelView = menuPanelView;
            _skinsController = skinsController;
        }
        
        public void Dispose()
        {
            _menuPanelView.StartGameButton.OnClick -= _menuSceneEntryPoint.StartGame;
            _menuPanelView.RemoveAdsButton.OnClick -= _inAppPurchaseService.BuyRemoveAds;
            _menuPanelView.SkinShopButton.OnClick -= OpenSkinShop;
            _menuPanelView.ClearSkinsData.OnClick -= _saveService.ClearSkinsData;
            
            if(_skinShopPanelPresenter != null)
                _skinShopPanelPresenter.OnBackToMenuButtonClick -= OpenMenu;
            if(_skinSlotViewPresenter != null)
                _skinSlotViewPresenter.OnSkinBought -= _skinSlotViewPresenter.HideView;
               
        }

        public void Initialize()
        {
            _menuPanelView.StartGameButton.OnClick += _menuSceneEntryPoint.StartGame;
            _menuPanelView.RemoveAdsButton.OnClick += _inAppPurchaseService.BuyRemoveAds;
            _menuPanelView.SkinShopButton.OnClick += OpenSkinShop;
            _menuPanelView.ClearSkinsData.OnClick += _saveService.ClearSkinsData;
            
            if(_dataCache.IsAdsRemoved)
                _menuPanelView.RemoveAdsButton.enabled = false;

            CheckDiscountSkin();
        }

        public void ShowView()
        {
            _menuPanelView.Show();
        }

        private void HideView()
        {
            _menuPanelView.Hide();
        }

        private void OpenSkinShop()
        {
            if (_skinShopPanelPresenter == null)
            {
                _skinShopPanelPresenter = _instantiator.Instantiate<SkinShopPanelPresenter>(
                    new[] { _uiFactory.CreatePanelView<SkinShopPanelPresenter>() });
                _skinShopPanelPresenter.Initialize();
                _skinShopPanelPresenter.OnBackToMenuButtonClick += OpenMenu;
            }

            HideView();
            _skinShopPanelPresenter.ShowView();
        }

        private void OpenMenu()
        {
            ShowView();
        }

        private void CheckDiscountSkin()
        {
            _skinSlotViewPresenter ??= new SkinSlotViewPresenter(_skinsController, _menuPanelView.DiscountSkinSlotView);
            
            if (_skinsController.BoughtSkins.Contains(_skinSlotViewPresenter.SkinSlotView.SkinEnum))
            {
                _skinSlotViewPresenter.HideView();
            }
            else
            {
                _skinSlotViewPresenter.Initialize();
                _skinSlotViewPresenter.ShowView();
                _skinSlotViewPresenter.OnSkinBought += _skinSlotViewPresenter.HideView;
            }
        }
    }
}