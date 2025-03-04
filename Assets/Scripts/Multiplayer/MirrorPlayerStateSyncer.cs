using System;
using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorPlayerStateSyncer : MirrorSingleton<MirrorPlayerStateSyncer>
    {
        public event Action OnPlayerDeath;

        public void CmdSetState(NetworkConnectionToClient conn, PlayerStateMessage newStateMessage)
        {
            RpcUpdateState(newStateMessage.StateName);

            if (newStateMessage.StateName == typeof(DeathState).ToString())
            {
                OnPlayerDeath?.Invoke();
            }
            
            Debug.Log($"[Server] {newStateMessage}");
        }
        
        [ClientRpc]
        private void RpcUpdateState(string newState)
        {
            Main.Player[] players = FindObjectsByType<Main.Player>(FindObjectsSortMode.None);

            foreach (var player in players)
            {
                player.PlayerStateMachine.SetState(newState);
            }
            
            Debug.Log($"[Client] {newState}");
        }
    }
}