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

        private Main.Player _player;

        private new void Awake()
        {
            base.Awake();

            _player = GetComponent<Main.Player>();
            
            _player.PlayerStateMachine.OnStateChanged += CmdSetState;
        }

        private void OnDestroy()
        {
            _player.PlayerStateMachine.OnStateChanged -= CmdSetState;
        }

        [Command]
        private void CmdSetState(PlayerStateMessage newStateMessage)
        {
            RpcUpdateState(netId, newStateMessage.StateName);

            if (newStateMessage.StateName == typeof(DeathState).ToString())
            {
                OnPlayerDeath?.Invoke();
            }

            Debug.Log($"[Server] {newStateMessage}");
        }

        [ClientRpc]
        public void RpcUpdateState(uint netID, string newState)
        {
            if (netID != netId)
                return;

            try
            {
                _player.PlayerStateMachine.SetState(newState);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            Debug.Log($"[Client] {newState}");
        }
    }
}