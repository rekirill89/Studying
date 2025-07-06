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

            _battleSceneAssetsLoader.OnBattleSceneAssetsPrepared += Init;
        }

        public void Dispose()
        {
            _battleSceneAssetsLoader.OnBattleSceneAssetsPrepared -= Init;
        }
        
        private void Init(BattleSettingsFacade _, Panels panels)
        {
            _presentersView = new Dictionary<Type, (BasePanelView, Transform)>()
            {
                {typeof(ContinuePanelPresenter), (panels.ContinuePanelView, _screenCanvasParent)},
                {typeof(StartPanelPresenter), (panels.StartPanelView, _screenCanvasParent)},
                {typeof(RestartPanelPresenter), (panels.RestartPanelView, _screenCanvasParent)},
                {typeof(SavePanelPresenter), (panels.SavePanelView, _screenCanvasParent)},
                {typeof(LoadPanelPresenter), (panels.LoadPanelView, _screenCanvasParent)},
                {typeof(ReloadPanelPresenter), (panels.ReloadPanelView, _hudCanvasParent)}
            };
        }

        public BasePanelView CreatePanelView(Type type)
        {
            if (!_presentersView.ContainsKey(type))
                Debug.LogError($"No view for type {type}");
            
            return Object.Instantiate(_presentersView[type].ViewPrefab,  _presentersView[type].Parent);
        }

    }
}