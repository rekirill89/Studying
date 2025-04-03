using UnityEngine;

namespace DuelGame
{
    public class BattleStateModel
    {
        public BattleState currentBattleState { get; private set; } = BattleState.NotStarted;
        
        public BaseHero player1 { get;  set; }
        public BaseHero player2 { get;  set; }

        public void SetState(BattleState newState)
        {
            currentBattleState = newState;
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

