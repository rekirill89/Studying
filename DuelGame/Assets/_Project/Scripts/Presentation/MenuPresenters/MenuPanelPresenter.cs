using TMPro;
using UnityEngine;
using Zenject;

namespace DuelGame
{
    public class MenuPanelPresenter : IPresenter<MenuView>
    {
        private readonly MenuSceneEntryPoint _menuSceneEntryPoint;
        private readonly IInAppPurchaseService _inAppPurchaseService;
        private readonly IInstantiator _instantiator;
        private readonly DataCache _dataCache;
        private readonly MenuView _menuView;
        private readonly UIFactory _uiFactory;
        private readonly SaveService _saveService;
        private readonly SkinsController _skinsController;
        
        private readonly InternetConnector _internetConnector;

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
            InternetConnector internetConnector,
            MenuView menuView)
        {
            _menuSceneEntryPoint = menuSceneEntryPoint;
            _inAppPurchaseService = inAppPurchaseService;
            _dataCache = dataCache;
            _uiFactory = uiFactory;
            _instantiator = instantiator;
            _saveService = saveService;
            _menuView = menuView;
            _internetConnector = internetConnector;
            _skinsController = skinsController;
        }
        
        public void Dispose()
        {
            _menuView.StartGameButton.OnClick -= _menuSceneEntryPoint.StartGame;
            _menuView.RemoveAdsButton.OnClick -= _inAppPurchaseService.BuyRemoveAds;
            _menuView.SkinShopButton.OnClick -= OpenSkinShop;
            _menuView.ClearSkinsData.OnClick -= _saveService.ClearSkinsData;
            
            if(_skinShopPanelPresenter != null)
                _skinShopPanelPresenter.OnBackToMenuButtonClick -= OpenMenu;
            if(_skinSlotViewPresenter != null)
                _skinSlotViewPresenter.OnSkinBought -= _skinSlotViewPresenter.HideView;
               
        }

        public void Initialize()
        {
            _menuView.StartGameButton.OnClick += _menuSceneEntryPoint.StartGame;
            _menuView.RemoveAdsButton.OnClick += _inAppPurchaseService.BuyRemoveAds;
            _menuView.SkinShopButton.OnClick += OpenSkinShop;
            _menuView.ClearSkinsData.OnClick += _saveService.ClearSkinsData;
            
            if(_dataCache.IsAdsRemoved)
                _menuView.RemoveAdsButton.enabled = false;
            if (!_internetConnector.IsConnected)
            {
                _menuView.RemoveAdsButton.SetInteractable(false);
            }

            CheckDiscountSkin();
        }

        public void ShowView()
        {
            _menuView.Show();
        }

        private void HideView()
        {
            _menuView.Hide();
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
            _skinSlotViewPresenter ??= new SkinSlotViewPresenter(_skinsController, _menuView.DiscountSkinSlotView);
            
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