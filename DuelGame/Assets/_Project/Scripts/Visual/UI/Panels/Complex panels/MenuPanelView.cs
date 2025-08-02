using UnityEngine;

namespace DuelGame
{
    public class MenuPanelView : BasePanelView
    {
        [field:SerializeField] public BaseButton StartGameButton { get;private set; }
        [field:SerializeField] public BaseButton RemoveAdsButton { get; private set; }
    }
}