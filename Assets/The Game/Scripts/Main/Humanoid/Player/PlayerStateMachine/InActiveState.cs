using Shadow_Dominion.AnimStateMachine;
using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class InActiveState : IState
    {
        private readonly Main.Player _player;
        private readonly PlayerAnimation _playerAnimation;
        private readonly Transform _ragdollRoot;
        
        public InActiveState(Main.Player player, PlayerAnimation playerAnimation, Transform ragdollRoot)
        {
            _player = player;
            _playerAnimation = playerAnimation;
            _ragdollRoot = ragdollRoot;
        }
        
        public override void Enter()
        {
            _ragdollRoot.gameObject.SetActive(false);
            
            _playerAnimation.AnimationStateMachine.SetState<AnimationIdleState>();

            Vector3 freePos = SpawnPointSyncer.Instance.GetFreePosition().Position;
            Quaternion rot = SpawnPointSyncer.Instance.CalculateRotation(_player.PlayersTrasform.position);
            
            _player.SetPositionAndRotation(SpawnPointSyncer.Instance.GetFreePosition().Position, 
                SpawnPointSyncer.Instance.CalculateRotation(_player.PlayersTrasform.position));
            
            _ragdollRoot.SetPositionAndRotation(freePos, rot);
            
            _ragdollRoot.gameObject.SetActive(true);
        }

        public override void Exit()
        {
            _ragdollRoot.gameObject.SetActive(true);
        }
    }
}