using Shadow_Dominion.AnimStateMachine;
using UnityEngine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class InActiveState : PlayerState
    {
        private readonly Main.MirrorPlayer _mirrorPlayer;
        
        public InActiveState(Main.MirrorPlayer mirrorPlayer, PlayerAnimation playerAnimation) : base(playerAnimation)
        {
            _mirrorPlayer = mirrorPlayer;
        }
        
        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.SetState<AnimationIdleState>();

            if (!_mirrorPlayer.isLocalPlayer) 
                return;
            
            Vector3 freePos = SpawnPointSyncer.Instance.GetFreePosition().Position;
            Quaternion rot = SpawnPointSyncer.Instance.CalculateRotation(freePos);
                
            _mirrorPlayer.SetRigidbodyPositionAndRotation(freePos, rot);
            _mirrorPlayer.SetRagdollPositionAndRotation(freePos, rot);
            
            _mirrorPlayer.SetCameraRotation(rot);

            _mirrorPlayer.SetRagdollVisibility(false);
            _mirrorPlayer.GetComponent<ActiveRagdollSetUp>().Disable();
            _mirrorPlayer.GetComponent<ActiveRagdollSetUp>().Enable();
                
            Debug.Log($"Free position: {freePos}");
        }

        public override void Exit()
        {
            _mirrorPlayer.SetRagdollVisibility(true);
        }

        public override bool CanExit() => true;
    }
}