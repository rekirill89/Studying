using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class UIFactory : IDisposable
    {
        public bool IsSystemReady { get; private set; } = false;

        private readonly Transform _screenCanvasParent;
        private readonly Transform _hudCanvasParent;
        private readonly BattleSceneAssetsLoader _battleSceneAssetsLoader; 

        private List<(BasePanelView viewPrefab, Transform parent)> _viewsCanvasList;
        
        public UIFactory(
            BattleSceneAssetsLoader battleSceneAssetsLoader,
            Transform screenCanvasParent,
            Transform hudCanvasParent)
        { 
            _screenCanvasParent = screenCanvasParent;
            _hudCanvasParent = hudCanvasParent;
            _battleSceneAssetsLoader = battleSceneAssetsLoader;
        }

        public void Initialize()
        {
            _battleSceneAssetsLoader.OnBattleSceneAssetsReady += Init;
        }
        
        public void Dispose()
        {
            _battleSceneAssetsLoader.OnBattleSceneAssetsReady -= Init;
        }
        
        private void Init(BattleSettingsFacade _, Panels panels)
        {
            Debug.Log("Presenters view Init");
            _viewsCanvasList = new List<(BasePanelView viewPrefab, Transform parent)>()
            {
                (panels.ContinuePanelView, _screenCanvasParent),
                (panels.StartPanelView, _screenCanvasParent),
                (panels.RestartPanelView, _screenCanvasParent),
                (panels.SavePanelView, _screenCanvasParent),
                (panels.LoadPanelView, _screenCanvasParent),
                (panels.ReloadPanelView, _hudCanvasParent),
                (panels.AdsPanelView, _screenCanvasParent)
            };
            IsSystemReady = true;
        }
        
        public BasePanelView CreatePanelView<T>() where T : IPresenter<BasePanelView>
        {
            var (prefab, parent) = GetViewPrefabNParentPair<T>();
            return Object.Instantiate(prefab,  parent);
        }
        
        private (BasePanelView prefab, Transform parent) GetViewPrefabNParentPair<TPresenter>() where TPresenter : IPresenter<BasePanelView>
        {
            var type = typeof(TPresenter)
                .GetInterfaces()
                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IPresenter<>))
                ?.GetGenericArguments()[0];
            var view = _viewsCanvasList.First(x => x.viewPrefab.GetType() == type);
            return (view.viewPrefab, view.parent);
        }
    }
}