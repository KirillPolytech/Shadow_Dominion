using Mirror;
using Multiplayer.Structs;
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

            _levelStateMachine.OnStateChanged += SendMessage;
            RegisterHandler();
        }

        ~MirrorLevelSyncer()
        {
            _levelStateMachine.OnStateChanged -= SendMessage;
            UnRegisterHandler();
        }

        private void SendMessage(IState state)
        {
            NetworkClient.Send(new LevelState(state.GetType().ToString()));
        }

        private void RegisterHandler()
        {
            NetworkServer.RegisterHandler<LevelState>(CmdSetState);
        }

        private void UnRegisterHandler()
        {
            NetworkServer.UnregisterHandler<LevelState>();
        }
        
        [Server]
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
}