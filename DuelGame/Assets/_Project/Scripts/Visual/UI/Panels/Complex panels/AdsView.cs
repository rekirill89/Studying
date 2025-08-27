using UnityEngine;

namespace DuelGame
{
    public class AdsView : BaseView
    {
        [field:SerializeField] public BaseButton CancelButton { get;private set; }
        [field:SerializeField] public BaseButton AcceptButton { get; private set; }
    }
}