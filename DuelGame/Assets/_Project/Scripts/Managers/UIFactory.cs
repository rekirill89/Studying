using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;
using IInitializable = Zenject.IInitializable;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class UIFactory : IDisposable
    {
        public bool IsSystemReady { get; private set; } = false;
        
        private readonly GlobalAssetsLoader _globalAssetsLoader; 

        private Panels _panels;
        
        private Canvas _screenCanvasPrefab;
        private Canvas _hudCanvasPrefab;
        private Canvas _screenCanvas;
        private Canvas _hudCanvas;
        
        private List<(BasePanelView viewPrefab, Transform parent)> _viewsCanvasList;
        
        public UIFactory(GlobalAssetsLoader globalAssetsLoader)
        { 
            _globalAssetsLoader = globalAssetsLoader;
        }

        public void Dispose()
        {
            _globalAssetsLoader.OnDataLoaded -= Initialize;
        }
        
        public void Init()
        {
            _globalAssetsLoader.OnDataLoaded += Initialize;
        }
        
        public BasePanelView CreatePanelView<T>() where T : IPresenter<BasePanelView>
        {
            CheckCanvases();
            var (prefab, parent) = GetViewPrefabAndParentPair<T>();
            return Object.Instantiate(prefab,  parent);
        }

        private void Initialize(GameLocalConfigs _, UILocalConfigs uiLocalConfigs)
        {
            Debug.Log("Presenters view Init");

            _screenCanvasPrefab = uiLocalConfigs.ScreenCanvas;
            _hudCanvasPrefab = uiLocalConfigs.HudCanvas;
            _panels = uiLocalConfigs.Panels;

            IsSystemReady = true;
            Debug.Log("Presenters view Init end");
        }
        
        private (BasePanelView prefab, Transform parent) GetViewPrefabAndParentPair<TPresenter>() where TPresenter : IPresenter<BasePanelView>
        {
            var type = typeof(TPresenter)
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPresenter<>))
                ?.GetGenericArguments()[0];
            var view = _viewsCanvasList.First(x => x.viewPrefab.GetType() == type);
            
            return (view.viewPrefab, view.parent);
        }

        private void CheckCanvases()
        {
            if (_hudCanvas == null && _screenCanvas == null)
            {
                _screenCanvas = Object.Instantiate(_screenCanvasPrefab);
                _hudCanvas = Object.Instantiate(_hudCanvasPrefab);
                
                _viewsCanvasList = new List<(BasePanelView viewPrefab, Transform parent)>()
                {
                    (_panels.ContinuePanelView, _screenCanvas.transform),
                    (_panels.StartPanelView, _screenCanvas.transform),
                    (_panels.RestartPanelView, _screenCanvas.transform),
                    (_panels.SavePanelView, _screenCanvas.transform),
                    (_panels.LoadPanelView, _screenCanvas.transform),
                    (_panels.ReloadPanelView, _hudCanvas.transform),
                    (_panels.AdsPanelView, _screenCanvas.transform),
                    (_panels.MenuPanelView, _screenCanvas.transform)
                };
            }
        }
    }
}