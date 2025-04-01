using System;
using Mirror;
using Multiplayer.Structs;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.Main
{
    public class MirrorPlayer : Humanoid
    {
        public event Action OnDead;
        public PlayerStateMachine PlayerStateMachine;
        
        public Transform AnimTransform { get; private set; }
        private Rigidbody _animRb;
        private Transform _ragdollTransform;
        private Rigidbody _ragdollRb;
        private CameraLook _cameraLook;

        public void Construct(
            Transform animTransform,
            Rigidbody animRb,
            Transform ragdollTransform,
            PlayerStateMachine playerStateMachine,
            CameraLook cameraLook)
        {
            AnimTransform = animTransform;
            _ragdollTransform = ragdollTransform;
            _animRb = animRb;
            PlayerStateMachine = playerStateMachine;
            _cameraLook = cameraLook;
            _ragdollRb = _ragdollTransform.GetComponent<Rigidbody>();
            
            PlayerStateMachine.OnStateChanged += CmdSetState;
        }

        private void OnDestroy()
        {
            PlayerStateMachine.OnStateChanged -= CmdSetState;
        }

        public void IsKinematic(bool isKinematic)
        {
            _animRb.isKinematic = isKinematic;
        }

        public void SetRigidbodyPositionAndRotation(Vector3 pos, Quaternion rot)
        {
            _animRb.position = pos;
            _animRb.rotation = rot;

            // Debug.LogWarning($"name: {_rigidbody.gameObject.name}, pos: {_rigidbody.position}, rot: {rot.eulerAngles}");
        }

        public void SetCameraRotation(Quaternion rot)
        {
            _cameraLook.SetRotation(rot);
        }

        public void SetRagdollPositionAndRotation(Vector3 pos, Quaternion rot)
        {
            //SetRagdollVisibility(false);
            _ragdollRb.transform.position = pos;
            _ragdollRb.transform.rotation = rot;
            //SetRagdollVisibility(true);
            
            Debug.Log($"Ragdoll new pos: {_ragdollRb.transform.position}");
        }

        public void SetRagdollVisibility(bool isVisible)
        {
            //_ragdollRb.gameObject.SetActive(isVisible);
        }

        #region Server

        [Command(requiresAuthority = false)]
        private void CmdSetState(PlayerStateMessage newStateMessage)
        {
            RpcUpdateState(newStateMessage.StateName);

            if (newStateMessage.StateName == typeof(DeathState).ToString())
            {
                OnDead?.Invoke();
            }

            // Debug.Log($"[Server] {newStateMessage.StateName}, Time: {Time.time}");
        }

        #endregion

        #region Client

        [ClientRpc]
        private void RpcUpdateState(string newState)
        {
            if (!isLocalPlayer)
                return;
            
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

        #endregion
    }
}