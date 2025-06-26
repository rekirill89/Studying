using System;
using Zenject;

namespace DuelGame
{
    public class ReloadPanelPresenter : IDisposable, IPresenter
    {
        private readonly BaseOneButtonPanelView _reloadPanelView;
        private readonly SceneLoaderService _sceneLoaderService;

        public ReloadPanelPresenter(
            ReloadPanelView reloadPanelView,
            SceneLoaderService sceneLoaderService)
        {
            _reloadPanelView = reloadPanelView;
            _sceneLoaderService = sceneLoaderService;
            
            _reloadPanelView.OnButtonClicked += _sceneLoaderService.LoadBattleScene;
        }
                
        public void Dispose()
        {
            _reloadPanelView.OnButtonClicked -= _sceneLoaderService.LoadBattleScene;

        }
        
        public void ShowView()
        {
            _reloadPanelView.Show();
        }
    }
}