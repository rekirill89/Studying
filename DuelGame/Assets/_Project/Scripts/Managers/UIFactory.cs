using UnityEngine;

namespace DuelGame
{
    public class UIFactory
    {
        private readonly HUDCanvasFacade _hudCanvasFacade;
        private readonly ScreenCanvasFacade _screenCanvasFacade;
        private readonly Transform _canvasParent;

        public UIFactory(HUDCanvasFacade hudCanvasFacade, ScreenCanvasFacade screenCanvasFacade, Transform canvasParent)
        {
            _hudCanvasFacade = hudCanvasFacade;
            _screenCanvasFacade = screenCanvasFacade;
            _canvasParent = canvasParent;
        }

        public HUDCanvasFacade CreateHUDCanvas()
        {
            return Object.Instantiate(_hudCanvasFacade, _canvasParent);
        }

        public ScreenCanvasFacade CreateScreenCanvas()
        {
            return Object.Instantiate(_screenCanvasFacade, _canvasParent);
        }
    }
}

