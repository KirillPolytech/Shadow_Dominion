using System;
using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Shadow_Dominion.Zombie;
using UnityEngine;

namespace Shadow_Dominion.Main
{
    public class Player : Humanoid, IZombieTarget
    {
        public event Action OnPlayerDeath;
        
        public Transform PlayersTrasform { get; set; }
        
        public PlayerStateMachine PlayerStateMachine;
        
        private Rigidbody _rigidbody;

        public void Construct(Transform t, PlayerStateMachine playerStateMachine)
        {
            PlayersTrasform = t;
            _rigidbody = t.GetComponent<Rigidbody>();
            PlayerStateMachine = playerStateMachine;
            
            PlayerStateMachine.OnStateChanged += CmdSetState;
        }

        private void OnDestroy()
        {
            PlayerStateMachine.OnStateChanged -= CmdSetState;
        }

        public void IsKinematic(bool iskinematic)
        {
            _rigidbody.isKinematic = iskinematic;
        }
        
        public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
        {
            _rigidbody.position = pos;
            _rigidbody.rotation = rot;
        }
        
        [Command]
        private void CmdSetState(PlayerStateMessage newStateMessage)
        {
            if (!isLocalPlayer) 
                Debug.LogWarning($"isLocalPlayer: {isLocalPlayer}");  
            
            if (!isOwned) 
                Debug.LogWarning($"isOwned: {isOwned}");           
            
            RpcUpdateState(newStateMessage.StateName);

            if (newStateMessage.StateName == typeof(DeathState).ToString())
            {
                OnPlayerDeath?.Invoke();
            }

            Debug.Log($"[Server] {newStateMessage}");
        }

        [ClientRpc]
        public void RpcUpdateState(string newState)
        {
            try
            {
                PlayerStateMachine.SetState(newState);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            Debug.Log($"[Client] {newState}");
        }
    }
}