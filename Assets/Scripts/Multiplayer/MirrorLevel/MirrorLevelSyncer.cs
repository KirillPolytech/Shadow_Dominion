using Mirror;
using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorLevelSyncer
    {
        private readonly LevelStateMachine _levelStateMachine;
        
        public MirrorLevelSyncer(LevelStateMachine levelStateMachine)
        {
            _levelStateMachine = levelStateMachine;
            
            RegisterHandler();
        }

        private void RegisterHandler()
        {
            NetworkServer.RegisterHandler<LevelState>(CmdSetState);
        }
        
        private void CmdSetState(NetworkConnectionToClient conn, LevelState newState)
        {
            RpcUpdateState(newState.StateName);
            
            Debug.Log($"[Server] {newState}");
        }
        
        [ClientRpc]
        private void RpcUpdateState(string newState)
        {
            _levelStateMachine.SetState(newState);
            
            Debug.Log($"[Client] {newState}");
        }
    }

    public struct LevelState : NetworkMessage
    {
        public string StateName;
        
        public LevelState(string stateName)
        {
            StateName = stateName;
        }
    }
}