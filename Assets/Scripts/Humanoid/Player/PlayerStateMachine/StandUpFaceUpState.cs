using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class StandUpFaceUpState : PlayerState
    {
        private readonly RigBuilder _rigBuilder;
        private readonly Main.Player _player;
        private readonly CameraLook _cameraLook;
        private readonly Transform _ragdollRoot;
        private readonly CoroutineExecuter _coroutineExecuter;
        private readonly PlayerStateMachine _playerStateMachine;
        private readonly float _clipLength;
        
        public StandUpFaceUpState(
            Main.Player player,
            Transform ragdollRoot,
            RigBuilder rigBuilder,
            PlayerAnimation playerAnimation,
            CameraLook cameraLook,
            CoroutineExecuter coroutineExecuter,
            PlayerStateMachine playerStateMachine,
            float clipLength) : base(playerAnimation)
        {
            _rigBuilder = rigBuilder;
            _player = player;
            _ragdollRoot = ragdollRoot;
            _cameraLook = cameraLook;
            _coroutineExecuter = coroutineExecuter;
            _playerStateMachine = playerStateMachine;
            _clipLength = clipLength;
        }
        
        public override void Enter()
        {
            _rigBuilder.enabled = false;
            _cameraLook.CanZooming = false;
            
            Vector3 pos = _ragdollRoot.position;
            Vector3 dirUp = new Vector3(_ragdollRoot.up.x, 0, _ragdollRoot.up.z);
            Quaternion q = Quaternion.LookRotation(-dirUp);
            _player.SetPositionAndRotation(pos, q);
            
            _playerAnimation.AnimationStateMachine.StandUpFaceUp();

            _coroutineExecuter.Execute(ExecutingCoroutine(_clipLength, _playerStateMachine.SetState<DefaultState>));
        }
        
        private IEnumerator ExecutingCoroutine(float waitTime, Action callBack)
        {
            yield return new WaitForSeconds(waitTime);

            callBack?.Invoke();
        }

        public override void Exit()
        {
            _rigBuilder.enabled = true;
            _cameraLook.CanZooming = true;
        }
    }
}