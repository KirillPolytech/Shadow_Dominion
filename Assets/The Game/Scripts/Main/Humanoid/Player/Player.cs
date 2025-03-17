using System;
using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.Main
{
    public class Player : Humanoid
    {
        public event Action OnDead;
        
        public Transform AnimTransform { get; private set; }
        public Rigidbody AnimRb { get; private set; }
        public Transform RagdollTransform { get; private set; }
        
        public PlayerStateMachine PlayerStateMachine;
        
        private CameraLook _cameraLook;

        public void Construct(
            Transform animTransform, 
            Rigidbody animRb, 
            Transform ragdollTransform, 
            PlayerStateMachine playerStateMachine, 
            CameraLook cameraLook)
        {
            AnimTransform = animTransform;
            RagdollTransform = ragdollTransform;
            AnimRb = animRb;
            PlayerStateMachine = playerStateMachine;
            _cameraLook = cameraLook;
            
            PlayerStateMachine.Initialize();
            
            PlayerStateMachine.OnStateChanged += CmdSetState;
        }

        private void OnDestroy()
        {
            PlayerStateMachine.OnStateChanged -= CmdSetState;
        }

        public void IsKinematic(bool iskinematic)
        {
            AnimRb.isKinematic = iskinematic;
        }
        
        public void SetRigidbodyPositionAndRotation(Vector3 pos, Quaternion rot)
        {
            _cameraLook.SetRotation(rot);

            AnimRb.position = pos;
            AnimRb.rotation = rot;
            
            // Debug.LogWarning($"name: {_rigidbody.gameObject.name}, pos: {_rigidbody.position}, rot: {rot.eulerAngles}");
        }
        
        [Command]
        private void CmdSetState(PlayerStateMessage newStateMessage)
        {
            RpcUpdateState(newStateMessage.StateName);

            if (newStateMessage.StateName == typeof(DeathState).ToString())
            {
                OnDead?.Invoke();
            }

            Debug.Log($"[Server] {newStateMessage}, Time: {Time.time}");
        }

        [ClientRpc]
        private void RpcUpdateState(string newState)
        {
            try
            {
                PlayerStateMachine.SetState(newState);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }

            // Debug.Log($"[Client] {newState} + Time: {Time.time}");
        }
    }
}