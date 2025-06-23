using UnityEngine;

namespace DuelGame
{
    public class UIFactory
    {        
        private readonly Transform _screenCanvasParent;
        private readonly Transform _hudCanvasParent;

        private readonly StartPanelView _startPanelView;
        private readonly ContinuePanelView _continuePanelView;
        private readonly RestartPanelView _restartPanelView;
        private readonly ReloadPanelView _reloadPanelView;
        private readonly SavePanelView _savePanelView;
        private readonly LoadPanelView _loadPanelView;

        public UIFactory(
            Transform screenCanvasParent,
            Transform hudCanvasParent,
            Panels panels)
        { 
            _startPanelView = panels.StartPanelView;
            _continuePanelView = panels.ContinuePanelView;
            _restartPanelView = panels.RestartPanelView;
            _reloadPanelView = panels.ReloadPanelView;
            _savePanelView = panels.SavePanelView;
            _loadPanelView = panels.LoadPanelView;
            
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
        
        public SavePanelView CreateSavePanelView()
        {
            return Object.Instantiate(_savePanelView, _screenCanvasParent);
        }
        
        public LoadPanelView CreateLoadPanelView()
        {
            return Object.Instantiate(_loadPanelView, _screenCanvasParent);
        }
        
        public ReloadPanelView CreateReloadPanelView()
        {
            return Object.Instantiate(_reloadPanelView, _hudCanvasParent);
        }
    }
}