using UnityEngine;

namespace DuelGame
{
    public class BasePanelView : MonoBehaviour
    {
        [SerializeField] protected GameObject Root;
        
        public void Show() => Root.SetActive(true);
        
        public void Hide() => Root.SetActive(false);
    }
}