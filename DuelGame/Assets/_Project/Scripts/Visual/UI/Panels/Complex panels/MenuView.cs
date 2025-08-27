using UnityEngine;

namespace DuelGame
{
    public class MenuView : BaseView
    {
        [field:SerializeField] public BaseButton StartGameButton { get;private set; }
        [field:SerializeField] public BaseButton RemoveAdsButton { get; private set; }
        [field:SerializeField] public BaseButton SkinShopButton { get; private set; }
        [field:SerializeField] public BaseButton ClearSkinsData { get; private set; }
        [field:SerializeField] public SkinSlotView DiscountSkinSlotView { get; private set; }
    }
}