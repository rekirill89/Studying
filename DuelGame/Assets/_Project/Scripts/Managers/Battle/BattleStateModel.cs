using System;
using UnityEngine;

namespace DuelGame
{
    public class BattleStateModel
    {
        public delegate void StateChanged(BattleState state);
        public event StateChanged OnStateChanged;
        public BattleState CurrentBattleState { get; private set; } = BattleState.NotStarted;

        public void SetState(BattleState newState)
        {
            CurrentBattleState = newState;
            OnStateChanged?.Invoke(CurrentBattleState);
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