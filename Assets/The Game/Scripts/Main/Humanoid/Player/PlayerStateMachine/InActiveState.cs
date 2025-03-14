using Shadow_Dominion.AnimStateMachine;
using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class InActiveState : IState
    {
        private readonly Main.Player _player;
        private readonly PlayerAnimation _playerAnimation;
        
        public InActiveState(Main.Player player, PlayerAnimation playerAnimation)
        {
            _player = player;
            _playerAnimation = playerAnimation;
        }
        
        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.SetState<AnimationIdleState>();

            Vector3 freePos = SpawnPointSyncer.Instance.GetFreePosition().Position;
            Quaternion rot = SpawnPointSyncer.Instance.CalculateRotation(freePos);
            
            _player.SetRigidbodyPositionAndRotation(freePos, rot);
        }

        public override void Exit()
        {
            _player.RagdollTransform.gameObject.SetActive(true);
        }
    }
}