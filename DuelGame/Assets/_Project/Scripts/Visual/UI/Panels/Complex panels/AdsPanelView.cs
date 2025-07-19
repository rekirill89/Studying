using UnityEngine;

namespace DuelGame
{
    public class AdsPanelView : BasePanelView
    {
        [field:SerializeField] public BaseButton CancelButton { get;private set; }
        [field:SerializeField] public BaseButton AcceptButton { get; private set; }
    }
}