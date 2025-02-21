using Mirror;
using Shadow_Dominion.Player.StateMachine;
using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorStateHandler : NetworkBehaviour
    {
        private PlayerStateMachine _playerStateMachine;
        
        public void Construct(PlayerStateMachine playerStateMachine)
        {
            _playerStateMachine = playerStateMachine;

            _playerStateMachine.OnStateChanged += Serialize;
        }

        private void OnDestroy()
        {
            _playerStateMachine.OnStateChanged -= Serialize; 
        }

        private void Serialize(IState newState)
        {
            CmdSetState(newState.GetType().ToString());
        }
        
        [Command]
        private void CmdSetState(string newState)
        {
            // Сообщаем всем клиентам
            RpcUpdateState(newState);
            
            Debug.Log($"[Server] {newState}");
        }
        
        [ClientRpc]
        private void RpcUpdateState(string newState)
        {
            _playerStateMachine.SetState(newState);
            
            Debug.Log($"[Clients] {newState}");
        }
    }
}