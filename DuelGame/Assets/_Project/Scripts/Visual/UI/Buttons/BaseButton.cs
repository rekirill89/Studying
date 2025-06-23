using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    [RequireComponent(typeof(Button))]
    public class BaseButton : MonoBehaviour
    {        
        public event Action OnClick;
        
        private Button _button;
        
        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(OnClickHandler);
        }

        private void OnDestroy()
        {
            _button.onClick.RemoveListener(OnClickHandler);
        }

        private void OnClickHandler()
        {
            OnClick?.Invoke();
        }
    }
}