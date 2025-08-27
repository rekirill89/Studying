using System;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class LoadView : BaseView
    {
        [field:SerializeField] public ChooseSaveView ChooseSaveView { get; private set; }
        [field:SerializeField] public BaseButton LoadDataButton { get; private set; }
    }
}