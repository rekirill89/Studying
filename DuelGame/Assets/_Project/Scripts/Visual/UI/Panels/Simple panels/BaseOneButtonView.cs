using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DuelGame
{
    public class BaseOneButtonView : BaseView
    {
        [SerializeField] protected Button Button;
                
        public event Action OnButtonClicked;
        
        public void Awake()
        {
            Button.onClick.AddListener(OnButtonClickedHandler);
        }

        private void OnDestroy()
        {
            Button.onClick.RemoveListener(OnButtonClickedHandler);
        }
        
        private void OnButtonClickedHandler()
        {
            OnButtonClicked?.Invoke();
        }
    }
}