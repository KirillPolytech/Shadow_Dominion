using Shadow_Dominion.AnimStateMachine;
using UnityEngine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class InActiveState : PlayerState
    {
        private readonly Main.Player _player;
        
        public InActiveState(Main.Player player, PlayerAnimation playerAnimation) : base(playerAnimation)
        {
            _player = player;
        }
        
        public override void Enter()
        {
            _player.RagdollTransform.gameObject.SetActive(false);
            
            _playerAnimation.AnimationStateMachine.SetState<AnimationIdleState>();

            Vector3 freePos = SpawnPointSyncer.Instance.GetFreePosition().Position;
            Quaternion rot = SpawnPointSyncer.Instance.CalculateRotation(freePos);
            
            _player.RagdollTransform.position = freePos;
            
            _player.SetRigidbodyPositionAndRotation(freePos, rot);
        }

        public override void Exit()
        {
            _player.RagdollTransform.gameObject.SetActive(true);
        }

        public override bool CanExit() => true;
    }
}