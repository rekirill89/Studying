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
        
        private List<(BaseView viewPrefab, Transform parent)> _viewsCanvasList;
        
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
        
        public BaseView CreatePanelView<T>() where T : IPresenter<BaseView>
        {
            CheckCanvases();
            var (prefab, parent) = GetViewPrefabAndParentPair<T>();
            return Object.Instantiate(prefab,  parent);
        }

        private void Initialize(GameLocalConfigs _, UILocalConfigs uiLocalConfigs)
        {
            _screenCanvasPrefab = uiLocalConfigs.ScreenCanvas;
            _hudCanvasPrefab = uiLocalConfigs.HudCanvas;
            _panels = uiLocalConfigs.Panels;

            IsSystemReady = true;
        }
        
        private (BaseView prefab, Transform parent) GetViewPrefabAndParentPair<TPresenter>() where TPresenter : IPresenter<BaseView>
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
                
                _viewsCanvasList = new List<(BaseView viewPrefab, Transform parent)>()
                {
                    (_panels.ContinueView, _screenCanvas.transform),
                    (_panels.StartView, _screenCanvas.transform),
                    (_panels.RestartView, _screenCanvas.transform),
                    (_panels.SaveView, _screenCanvas.transform),
                    (_panels.LoadView, _screenCanvas.transform),
                    (_panels.ReloadView, _hudCanvas.transform),
                    (_panels.AdsView, _screenCanvas.transform),
                    (_panels.MenuView, _screenCanvas.transform),
                    (_panels.SkinShopView, _screenCanvas.transform),
                    (_panels.BattleFinishView, _screenCanvas.transform)
                };
            }
        }
    }
}