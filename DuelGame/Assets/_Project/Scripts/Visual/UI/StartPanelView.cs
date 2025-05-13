using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class StartPanelView : MonoBehaviour, IView
    {
        [SerializeField] private GameObject _root;
        [SerializeField] private Button _startButton;
        
        public void Show() => _root.SetActive(true);
        public void Hide() => _root.SetActive(false);
        
        public  event Action OnButtonClicked;

        private void Awake()
        {
            _startButton.onClick.AddListener(() => OnButtonClicked?.Invoke());
        }
    }   
}
