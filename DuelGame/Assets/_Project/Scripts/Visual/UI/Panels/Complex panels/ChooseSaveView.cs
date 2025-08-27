using DuelGame;
using UnityEngine;
using UnityEngine.UI;

namespace DuelGame
{
    public class ChooseSaveView : BaseView
    {
        [field:SerializeField] public BaseButton LoadAutoSaveButton { get; private set; }
        [field:SerializeField] public BaseButton LoadCloudSaveButton { get; private set; }
    }
}