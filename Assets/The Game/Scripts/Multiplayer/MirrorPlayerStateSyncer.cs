using System;
using System.Linq;
using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorPlayerStateSyncer : MirrorSingleton<MirrorPlayerStateSyncer>
    {
        public event Action OnPlayerDeath;

        public void RegisterHandler()
        {
            NetworkServer.RegisterHandler<PlayerStateMessage>(CmdSetState);
        }
        
        private void OnDestroy()
        {
            NetworkServer.UnregisterHandler<PlayerStateMessage>();
        }

        private void CmdSetState(NetworkConnectionToClient conn, PlayerStateMessage newStateMessage)
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
            Main.Player[] players = MirrorServer
                .Instance.Connections.Select(x => x.identity.GetComponent<Main.Player>()).ToArray();//FindObjectsByType<Main.Player>(FindObjectsSortMode.None);

            foreach (var player in players)
            {
                try
                {
                    player.PlayerStateMachine.SetState(newState);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            
            Debug.Log($"[Client] {newState}");
        }
    }
}