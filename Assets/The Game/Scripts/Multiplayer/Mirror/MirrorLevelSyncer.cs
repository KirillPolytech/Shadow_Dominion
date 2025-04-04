using System;
using Multiplayer.Structs;
using Shadow_Dominion.Main;
using Shadow_Dominion.StateMachine;

namespace Shadow_Dominion
{
    public class MirrorLevelSyncer : Singleton<MirrorLevelSyncer>
    {
        public Action<LevelState> OnUpdate;   
        
        private LevelStateMachine _levelStateMachine;
        
        public MirrorLevelSyncer(LevelStateMachine levelStateMachine)
        {
            _levelStateMachine = levelStateMachine;
            
            if (Instance == null)
                Instance = this;
            else
                throw new Exception("Second instance of MirrorLevelSyncer.");
        }

        public void Initialize(LevelStateMachine levelStateMachine)
        {
            _levelStateMachine = levelStateMachine;

            _levelStateMachine.OnStateChanged += OnStateUpdate;
        }

        ~MirrorLevelSyncer()
        {
            Instance = null;
            
            _levelStateMachine.OnStateChanged -= OnStateUpdate;
        }

        private void OnStateUpdate(IState state)
        {
            OnUpdate?.Invoke(new LevelState(state.GetType().ToString()));
        }
        
        public void SetState(LevelState newState)
        {
            _levelStateMachine.SetState(newState.StateName);
        }
    }
}