using System.Collections;
using System.Linq;
using Shadow_Dominion.AnimStateMachine;
using Shadow_Dominion.InputSystem;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player.StateMachine
{
    public class RagdollState : PlayerState
    {
        private readonly IInputHandler _inputHandler;
        private readonly Rigidbody _animRb;

        private readonly PlayerStateMachine _playerStateMachine;
        private readonly RigBuilder _rigBuilder;
        private readonly BoneController[] _boneControllers;
        private readonly CameraLook _cameraLook;
        private readonly Transform _ragdollRoot;
        private readonly Vector3 _forceDirection;
        private readonly Main.Player _player;
        private readonly Ak47 _ak47;
        private readonly CoroutineExecuter _coroutineExecuter;
        private readonly PlayerSettings _playerSettings;
        
        public RagdollState(
            PlayerAnimation playerAnimation,
            CameraLook cameraLook,
            RigBuilder rigBuilder,
            BoneController[] boneControllers,
            IInputHandler inputHandler,
            Transform ragdollRoot,
            PlayerStateMachine playerStateMachine,
            Main.Player player,
            Rigidbody animRb,
            Ak47 ak47,
            CoroutineExecuter coroutineExecuter,
            PlayerSettings playerSettings) : base(playerAnimation)
        {
            _rigBuilder = rigBuilder;
            _boneControllers = boneControllers;
            _cameraLook = cameraLook;
            _forceDirection = Vector3.zero;
            _inputHandler = inputHandler;
            _ragdollRoot = ragdollRoot;
            _playerStateMachine = playerStateMachine;
            _player = player;
            _animRb = animRb;
            _ak47 = ak47;
            _coroutineExecuter = coroutineExecuter;
            _playerSettings = playerSettings;
        }

        public override void Enter()
        {
            BoneController hand = _boneControllers.First(x => x.BoneType == HumanBodyBones.RightHand);
            Transform rightHand = hand.transform;
            
            _ak47.SetParent(rightHand);
            _ak47.SetRagdollTransform();
            
            _playerAnimation.AnimationStateMachine.SetState<AnimationLay>();

            _rigBuilder.enabled = false;
            _cameraLook.CanZooming = false;

            for (int i = 0; i < _boneControllers.Length; i++)
            {
                _boneControllers[i].IsPositionApplying(false);
                _boneControllers[i].IsRotationApplying(false);
                _boneControllers[i].AddForce(_forceDirection);
            }

            _player.IsKinematic(true);

            _coroutineExecuter.Execute(WaitForSeconds());
        }
        
        private IEnumerator WaitForSeconds()
        {
            float counter = 0;
            
            while (counter < _playerSettings.RagdollDelay)
            {
                counter += Time.fixedDeltaTime;
                MoveAnimPlayerToRagdollPos();
                yield return new WaitForFixedUpdate();
            }
            
            if (Vector3.Dot(_ragdollRoot.forward, Vector3.up) > 0)
                _playerStateMachine.SetState<StandUpFaceUpState>();
            else
                _playerStateMachine.SetState<StandUpFaceDownState>();
        }

        private void MoveAnimPlayerToRagdollPos()
        {
            bool isUp = Vector3.Dot(_ragdollRoot.forward, Vector3.up) > 0;

            Vector3 dirUp = new Vector3(_ragdollRoot.up.x, 0, _ragdollRoot.up.z);
            Quaternion rot = Quaternion.LookRotation(dirUp * (isUp ? -1 : 1));
            float y = _player.AnimTransform.position.y;

            _player.IsKinematic(true);

            Vector3 a = _ragdollRoot.position;
            a.y = y;
            Vector3 b = _player.AnimTransform.position;
            b.y = y;

            Vector3 pos = Vector3.Lerp(a, b, Time.fixedDeltaTime * Time.fixedDeltaTime);
            rot = Quaternion.Lerp(rot, _ragdollRoot.rotation, Time.fixedDeltaTime * Time.fixedDeltaTime);
            _player.SetRigidbodyPositionAndRotation(pos, rot);
        }

        public override void Exit()
        {
            _player.IsKinematic(false);
        }

        public override bool CanExit() => true;
    }
}