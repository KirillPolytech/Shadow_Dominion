using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.Main
{
    public class PlayerMovement
    {
        private PlayerStateMachine _playerStateMachine;
        private PlayerSettings _playerSettings;
        private CameraLook _cameraLook;
        private Rigidbody _charRigidbody;
        private PlayerAnimation _playerAnimation;

        private Transform _transform;
        private Transform _ragdollRoot;
        private Quaternion _cachedRot;

        public void Construct(
            PlayerStateMachine playerStateMachine,
            PlayerSettings playerSettings,
            Rigidbody characterController,
            CameraLook cameraLook,
            Transform ragdollRoot,
            PlayerAnimation playerAnimation)
        {
            _playerStateMachine = playerStateMachine;
            _playerSettings = playerSettings;
            _charRigidbody = characterController;
            _cameraLook = cameraLook;
            _transform = _charRigidbody.transform;
            _ragdollRoot = ragdollRoot;
            _playerAnimation = playerAnimation;
        }

        public void HandleInput(InputData data, bool isLocalPlayer)
        {
            if (!isLocalPlayer)
                return;

            Move(data.LeftShift, data.HorizontalAxisRaw, data.VerticalAxisRaw);
            Rotate();
            HandleAnim(data);
        }
        
        private void HandleAnim(InputData data)
        {
            int leftShift = data.LeftShift ? 1 : 0;
            
            // todo: refactor
            _playerAnimation.AnimationStateMachine.SetXY(data.HorizontalAxisRaw, data.VerticalAxisRaw  / 2 + 0.5f * leftShift);
        }

        private void Move(bool isRun, float x, float y)
        {
            if (!_playerSettings.canMove)
                return;

            int isRunInt = isRun ? 1 : 0;

            Vector3 dir = (_cameraLook.CameraTransform.forward * y +
                            _cameraLook.CameraTransform.right * x).normalized;
            dir *= _playerSettings.walkSpeed * (1 - isRunInt) + _playerSettings.runSpeed * isRunInt;
            dir.y = 0;

            _charRigidbody.linearVelocity = dir;

            Debug.DrawRay(_transform.position + Vector3.up, dir * 10, Color.red);
            Debug.DrawRay(_transform.position, _charRigidbody.linearVelocity * 10, Color.yellow);
        }

        private void Rotate()
        {
            if (!_playerSettings.canRotate)
                return;

            Vector3 transformForward =
                new Vector3(_cameraLook.CameraTransform.forward.x, 0, _cameraLook.CameraTransform.forward.z);

            //transformForward.y = dir == default ? 0 : Mathf.Sign(dir.y) * _playerSettings.tilt;

            Quaternion rot = Quaternion.Lerp(_charRigidbody.rotation,
                Quaternion.LookRotation(transformForward),
                _playerSettings.rotSpeed * Time.fixedDeltaTime);

            _charRigidbody.MoveRotation(rot);
        }

        public void StandUp(InputData data)
        {
            if (!data.F_Down)
                return;

            if (Vector3.Dot(_ragdollRoot.forward, Vector3.up) > 0)
                _playerStateMachine.SetState<StandUpFaceUpState>();
            else
                _playerStateMachine.SetState<StandUpFaceDownState>();
        }
    }
}