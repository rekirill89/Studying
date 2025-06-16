using System;
using Zenject;

namespace DuelGame
{
    public class ReloadPanelPresenter : IDisposable
    {
        private readonly ReloadPanelView _reloadPanelView;
        private readonly SceneLoaderManager _sceneLoaderManager;

        public ReloadPanelPresenter(
            ReloadPanelView reloadPanelView,
            SceneLoaderManager sceneLoaderManager)
        {
            _reloadPanelView = reloadPanelView;
            _sceneLoaderManager = sceneLoaderManager;
            
            _reloadPanelView.OnButtonClicked += _sceneLoaderManager.LoadBattleScene;
        }
                
        public void Dispose()
        {
            _reloadPanelView.OnButtonClicked -= _sceneLoaderManager.LoadBattleScene;

        }
        
        public void ShowView()
        {
            _reloadPanelView.Show();
        }
    }
}