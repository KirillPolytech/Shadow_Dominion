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

            Vector3 freePos = SpawnPointSyncer.Instance.GetFreePosition().Position;
            Quaternion rot = SpawnPointSyncer.Instance.CalculateRotation(freePos);
            
            _mirrorPlayer.SetRigidbodyPositionAndRotation(freePos, rot);
            _mirrorPlayer.SetRagdollPositionAndRotation(freePos, rot);
        }

        public override void Exit()
        {
        }

        public override bool CanExit() => true;
    }
}