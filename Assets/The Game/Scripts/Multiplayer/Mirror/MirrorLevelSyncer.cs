using System;
using Multiplayer.Structs;
using Shadow_Dominion.Main;
using Shadow_Dominion.StateMachine;

namespace Shadow_Dominion
{
    public class MirrorLevelSyncer : Singleton<MirrorLevelSyncer>
    {
        private readonly LevelStateMachine _levelStateMachine;
        
        public MirrorLevelSyncer(LevelStateMachine levelStateMachine)
        {
            _levelStateMachine = levelStateMachine;

            if (Instance == null)
                Instance = this;
            else
                throw new Exception("Second instance of MirrorLevelSyncer.");
        }
        
        public void CmdSetState(LevelState newState)
        {
            _levelStateMachine.SetState(newState.StateName);
        }
    }
}