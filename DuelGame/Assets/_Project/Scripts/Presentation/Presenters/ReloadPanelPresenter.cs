using System;
using Zenject;

namespace DuelGame
{
    public class ReloadPanelPresenter : IPresenter<ReloadView>
    {
        private readonly BaseOneButtonView _reloadView;
        private readonly SceneLoaderService _sceneLoaderService;

        public ReloadPanelPresenter(
            ReloadView reloadView,
            SceneLoaderService sceneLoaderService)
        {
            _reloadView = reloadView;
            _sceneLoaderService = sceneLoaderService;
        }
                               
        public void Initialize()
        {
            _reloadView.OnButtonClicked += _sceneLoaderService.LoadBattleScene;
        }
 
        public void Dispose()
        {
            _reloadView.OnButtonClicked -= _sceneLoaderService.LoadBattleScene;

        }
        
        public void ShowView()
        {
            _reloadView.Show();
        }
    }
}