using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class StandUpFaceDownState : PlayerState
    {
        private readonly RigBuilder _rigBuilder;
        private readonly Main.Player _player;
        private readonly CameraLook _cameraLook;
        private readonly Transform _ragdollRoot;
        private readonly CoroutineExecuter _coroutineExecuter;
        private readonly PlayerStateMachine _playerStateMachine;
        
        public StandUpFaceDownState(
            Main.Player player,
            Transform ragdollRoot,
            RigBuilder rigBuilder,
            PlayerAnimation playerAnimation,
            CameraLook cameraLook,
            CoroutineExecuter coroutineExecuter,
            PlayerStateMachine playerStateMachine) : base(playerAnimation)
        {
            _rigBuilder = rigBuilder;
            _player = player;
            _ragdollRoot = ragdollRoot;
            _cameraLook = cameraLook;
            _coroutineExecuter = coroutineExecuter;
            _playerStateMachine = playerStateMachine;
        }
        
        public override void Enter()
        {
            _rigBuilder.enabled = false;
            _cameraLook.CanZooming = false;

            Vector3 pos = _ragdollRoot.position;
            Vector3 dirUp = new Vector3(_ragdollRoot.up.x, 0, _ragdollRoot.up.z);
            Quaternion q = Quaternion.LookRotation(dirUp);
            _player.SetPositionAndRotation(pos, q);
            
            _playerAnimation.AnimationStateMachine.StandUpFaceDown();

            //var clip = _playerAnimation.Animator.GetCurrentAnimatorClipInfo(0)[0].clip;
            //float length = clip.length;
            _coroutineExecuter.StartCoroutine(8, _playerStateMachine.SetState<DefaultState>);
        }

        public override void Exit()
        {
            _rigBuilder.enabled = true;
            _cameraLook.CanZooming = true;
        }
    }
}