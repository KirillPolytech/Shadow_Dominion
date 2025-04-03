using Shadow_Dominion.AnimStateMachine;
using UnityEngine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class InActiveState : PlayerState
    {
        private readonly Main.MirrorPlayer _mirrorPlayer;
        private readonly CameraLook _cameraLook;
        
        public InActiveState(Main.MirrorPlayer mirrorPlayer, PlayerAnimation playerAnimation, CameraLook cameraLook) : base(playerAnimation)
        {
            _mirrorPlayer = mirrorPlayer;
            _cameraLook = cameraLook;
        }
        
        public override void Enter()
        {
            _cameraLook.CanRotate(false);
            
            _playerAnimation.AnimationStateMachine.SetState<AnimationIdleState>();
            
            Vector3 freePos = SpawnPointSyncer.Instance.GetFreePosition(MirrorPlayersSyncer.Instance.LocalPlayer.ID);
            Quaternion rot = SpawnPointSyncer.Instance.CalculateRotation(freePos);
            
            _mirrorPlayer.SetRigidbodyPositionAndRotation(freePos, rot);
            _mirrorPlayer.SetRagdollPositionAndRotation(freePos, rot);
            
            _mirrorPlayer.SetCameraRotation(rot);
            
            //Debug.Log($"Free position: {freePos}");
        }

        public override void Exit()
        {
            _cameraLook.CanRotate(true);
        }

        public override bool CanExit() => true;
    }
}