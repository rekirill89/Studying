using System;

namespace DuelGame
{
    public class ReloadButtonPresenter : IDisposable
    {
        private readonly ReloadButtonView _reloadButtonView;
        private readonly SceneLoaderManager _sceneLoaderManager;

        public ReloadButtonPresenter(
            ReloadButtonView reloadButtonView,
            SceneLoaderManager sceneLoaderManager)
        {
            _reloadButtonView = reloadButtonView;
            _sceneLoaderManager = sceneLoaderManager;

            _reloadButtonView.OnButtonClicked += _sceneLoaderManager.LoadBattleScene;
        }

        public void Dispose()
        {
            _reloadButtonView.OnButtonClicked -= _sceneLoaderManager.LoadBattleScene;

        }
    }
}