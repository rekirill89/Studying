using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DuelGame
{
    public class UIFactory
    {        
        private readonly Dictionary<Type, (BasePanelView ViewPrefab, Transform Parent)> _presentersView;

        public UIFactory(
            Transform screenCanvasParent,
            Transform hudCanvasParent,
            Panels panels)
        { 

            _presentersView = new Dictionary<Type, (BasePanelView, Transform)>()
            {
                {typeof(ContinuePanelPresenter), (panels.ContinuePanelView, screenCanvasParent)},
                {typeof(StartPanelPresenter), (panels.StartPanelView, screenCanvasParent)},
                {typeof(RestartPanelPresenter), (panels.RestartPanelView, screenCanvasParent)},
                {typeof(SavePanelPresenter), (panels.SavePanelView, screenCanvasParent)},
                {typeof(LoadPanelPresenter), (panels.LoadPanelView, screenCanvasParent)},
                {typeof(ReloadPanelPresenter), (panels.ReloadPanelView, hudCanvasParent)}
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