using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class UIFactory : IDisposable
    {
        public bool IsSystemReady { get; private set; } = false;

        private readonly Transform _screenCanvasParent;
        private readonly Transform _hudCanvasParent;
        private readonly BattleSceneAssetsLoader _battleSceneAssetsLoader; 

        private Dictionary<Type, (BasePanelView ViewPrefab, Transform Parent)> _presentersView;
        
        public UIFactory(
            BattleSceneAssetsLoader battleSceneAssetsLoader,
            Transform screenCanvasParent,
            Transform hudCanvasParent)
        { 
            _screenCanvasParent = screenCanvasParent;
            _hudCanvasParent = hudCanvasParent;
            _battleSceneAssetsLoader = battleSceneAssetsLoader;

            _battleSceneAssetsLoader.OnBattleSceneAssetsReady += Init;
            Debug.Log("Subscribed");
        }

        public void Dispose()
        {
            _battleSceneAssetsLoader.OnBattleSceneAssetsReady -= Init;
        }
        
        private void Init(BattleSettingsFacade _, Panels panels)
        {
            Debug.Log("Presenters view created");
            _presentersView = new Dictionary<Type, (BasePanelView, Transform)>()
            {
                {typeof(ContinuePanelPresenter), (panels.ContinuePanelView, _screenCanvasParent)},
                {typeof(StartPanelPresenter), (panels.StartPanelView, _screenCanvasParent)},
                {typeof(RestartPanelPresenter), (panels.RestartPanelView, _screenCanvasParent)},
                {typeof(SavePanelPresenter), (panels.SavePanelView, _screenCanvasParent)},
                {typeof(LoadPanelPresenter), (panels.LoadPanelView, _screenCanvasParent)},
                {typeof(ReloadPanelPresenter), (panels.ReloadPanelView, _hudCanvasParent)},
                {typeof(AdsPanelPresenter), (panels.AdsPanelView, _screenCanvasParent)}
            };
            IsSystemReady = true;
        }

        public BasePanelView CreatePanelView<T>()
        {
            Type type = typeof(T);
            Debug.Log($"{_presentersView.ContainsKey(type)}, {_presentersView.Count}");
            if (!_presentersView.ContainsKey(type))
                Debug.LogError($"No view for type {type}");
            return Object.Instantiate(_presentersView[type].ViewPrefab,  _presentersView[type].Parent);
        }
    }
}