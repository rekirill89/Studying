using TMPro;

namespace DuelGame
{
    public class MenuPanelPresenter : IPresenter<MenuPanelView>
    {
        private readonly MenuSceneEntryPoint _menuSceneEntryPoint;
        private readonly IInAppPurchaseService _inAppPurchaseService;
        private readonly DataCache _dataCache;
        private readonly MenuPanelView _view;
        
        public MenuPanelPresenter(
            MenuSceneEntryPoint menuSceneEntryPoint, 
            IInAppPurchaseService inAppPurchaseService, 
            DataCache dataCache,
            MenuPanelView view)
        {
            _menuSceneEntryPoint = menuSceneEntryPoint;
            _inAppPurchaseService = inAppPurchaseService;
            _dataCache = dataCache;
            _view = view;
        }
        
        public void Initialize()
        {
            _view.StartGameButton.OnClick += _menuSceneEntryPoint.StartGame;
            _view.RemoveAdsButton.OnClick += _inAppPurchaseService.BuyRemoveAds;
            
            if(_dataCache.IsAdsRemoved)
                _view.RemoveAdsButton.enabled = false;
        }

        public void Dispose()
        {
            _view.StartGameButton.OnClick -= _menuSceneEntryPoint.StartGame;
            _view.RemoveAdsButton.OnClick -= _inAppPurchaseService.BuyRemoveAds;
        }

        public void ShowView()
        {
            _view.Show();
        }
    }
}