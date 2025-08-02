using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class LoadPanelView : BasePanelView
    {
        [field:SerializeField] public BaseButton LoadDataButton { get;private set; }
    }
}