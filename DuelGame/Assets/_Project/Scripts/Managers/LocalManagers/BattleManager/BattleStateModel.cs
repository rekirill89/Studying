using UnityEngine;

namespace DuelGame
{
    public class BattleStateModel
    {
        public BattleState CurrentBattleState { get; private set; } = BattleState.NotStarted;

        public void SetState(BattleState newState)
        {
            CurrentBattleState = newState;
        }
    }
    
    public enum BattleState
    {
        NotStarted,
        Started,
        Continued,
        Finished
    }
}