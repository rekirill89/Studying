using UnityEngine;

namespace DuelGame
{
    public class SkinShopPanelView : BasePanelView
    {
        [field:SerializeField] public SkinSlotView SkinSlotView1 { get; private set; }
        [field:SerializeField] public SkinSlotView SkinSlotView2 { get; private set; }
        [field:SerializeField] public SkinSlotView SkinSlotView3 { get; private set; }
        [field:SerializeField] public BaseButton BackToMenuButton { get; private set; }
    }
}