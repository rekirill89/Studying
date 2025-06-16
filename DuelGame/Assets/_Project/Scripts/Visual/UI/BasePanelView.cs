using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DuelGame
{
    public class BasePanelView : MonoBehaviour
    {
        [SerializeField] protected GameObject Root;
        [SerializeField] protected Button Button;
                
        public event Action OnButtonClicked;
        
        public void Show() => Root.SetActive(true);
        
        public void Hide() => Root.SetActive(false);
    
        private void Awake()
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