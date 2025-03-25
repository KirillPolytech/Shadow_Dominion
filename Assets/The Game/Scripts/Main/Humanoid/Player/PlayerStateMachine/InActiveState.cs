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
            _mirrorPlayer.RagdollTransform.gameObject.SetActive(false);
            
            _playerAnimation.AnimationStateMachine.SetState<AnimationIdleState>();

            Vector3 freePos = SpawnPointSyncer.Instance.GetFreePosition().Position;
            Quaternion rot = SpawnPointSyncer.Instance.CalculateRotation(freePos);
            
            _mirrorPlayer.RagdollTransform.position = freePos;
            
            _mirrorPlayer.SetRigidbodyPositionAndRotation(freePos, rot);
        }

        public override void Exit()
        {
            _mirrorPlayer.RagdollTransform.gameObject.SetActive(true);
        }

        public override bool CanExit() => true;
    }
}