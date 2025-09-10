using UnityEngine;

namespace DuelGame
{
    public class BattleFinishView : BaseView
    {
        [field:SerializeField] public YouDiedView YouDiedView { get; private set; }
        [field:SerializeField] public EnemyFelledView EnemyFelledView { get; private set; }
    }
}