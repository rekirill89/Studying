using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class BasePanelView : MonoBehaviour
    {
        [SerializeField] protected GameObject _root;
        [SerializeField] protected Button _button;
                
        public event Action OnButtonClicked;
        
        public void Show() => _root.SetActive(true);
        
        public void Hide() => _root.SetActive(false);
    
        private void Awake()
        {
            _button.onClick.AddListener(() => OnButtonClicked?.Invoke());
        }
    }
}

