using UnityEngine;

namespace DuelGame
{
    public class UIFactory
    {
        private StartPanelView _startPanelView;
        private ContinuePanelView _continuePanelView;
        private RestartPanelView _restartPanelView;
        private ReloadButtonView _reloadButtonView;
        
        private readonly Transform _screenCanvasParent;
        private readonly Transform _hudCanvasParent;

        public UIFactory(
            Transform screenCanvasParent,
            Transform hudCanvasParent,
            StartPanelView startPanelView,
            ContinuePanelView continuePanelView,
            RestartPanelView restartPanelView,
            ReloadButtonView reloadButtonView)
        { 
            _startPanelView = startPanelView;
            _continuePanelView = continuePanelView;
            _restartPanelView = restartPanelView;
            _reloadButtonView = reloadButtonView;
            
            _screenCanvasParent = screenCanvasParent;
            _hudCanvasParent = hudCanvasParent;
        }

        public StartPanelView CreateStartPanelView()
        {
            return Object.Instantiate(_startPanelView, _screenCanvasParent);
        }

        public ContinuePanelView CreateContinuePanelView()
        {
            return Object.Instantiate(_continuePanelView, _screenCanvasParent);
        }

        public RestartPanelView CreateRestartPanelView()
        {
            return Object.Instantiate(_restartPanelView, _screenCanvasParent);
        }

        public ReloadButtonView CreateReloadButtonView()
        {
            return Object.Instantiate(_reloadButtonView, _hudCanvasParent);
        }
    }
}

