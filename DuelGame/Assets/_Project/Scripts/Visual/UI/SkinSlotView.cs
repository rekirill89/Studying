using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class SkinSlotView : BaseView
    {
        [field:SerializeField] public BaseButton EquipSkinButton { get; private set; }
        [field:SerializeField] public BaseButton UnEquipSkinButton { get; private set; }
        [field:SerializeField] public BaseButton BuySkinButton { get; private set; }
        [field:SerializeField] public HeroEnum HeroEnum { get; private set; }
        [field:SerializeField] public SkinEnum SkinEnum { get; private set; }
    }
}