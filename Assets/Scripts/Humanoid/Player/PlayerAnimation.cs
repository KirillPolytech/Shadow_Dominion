using System;
using System.Collections;
using Mirror;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player
{
    public class PlayerAnimation : NetworkBehaviour
    {
        private const float Appromixation = 0.01f;
        private const float VelocityApproximation = 1.5f;

        public bool CanAnimate { get; set; } = true;

        public AnimationStateMachine AnimationStateMachine { get; private set; }

        public Animator Animator { get; private set; }

        [Range(0, 10f)]
        [SerializeField]
        private float aimRigWeightChangeCoeff = 3.5f;

        private MonoInputHandler _inputHandler;
        private PlayerStateMachine _playerStateMachine;
        private Rig _aimRig;
        private Rigidbody _ragdoll;

        private Coroutine _coroutine;
        private int _lastValue;
        
        private const float TimeToWait = 7;
        private float _counter;
        private bool _isStandUp;
        private InputData _inputData;

        public void Construct(
            Animator animator,
            MonoInputHandler inputHandler,
            Rig aimRig,
            Rigidbody ragdoll,
            PlayerStateMachine playerStateMachine)
        {
            Animator = animator;
            _inputHandler = inputHandler;
            _aimRig = aimRig;
            _playerStateMachine = playerStateMachine;
            _ragdoll =ragdoll;

            AnimationStateMachine = new AnimationStateMachine(Animator);
        }

        private void OnEnable()
        {
            if (!isLocalPlayer)
                return;

            _inputHandler.OnInputUpdate += ReceiveInput;
        }
        
        public void OnDisable()
        {
            if (isLocalPlayer)
                return;
            
            _inputHandler.OnInputUpdate -= ReceiveInput;
        }

        private void ReceiveInput(InputData inputData) => _inputData = inputData;

        private void FixedUpdate()
        {
            float magnitude = _ragdoll.linearVelocity.magnitude;
            
            HandleAimRig();
            HandleWalkState(magnitude);
            HandleRunStates(magnitude);
            HandleHorizontalState(magnitude);
            HandleIdleState(magnitude);
        }

        private void HandleAimRig()
        {
            if (!CanAnimate || _isStandUp)
                return;

            int currentValue = _inputData.RightMouseButton ? 1 : 0;

            if (_lastValue != currentValue)
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);

                _coroutine = StartCoroutine(ChangeWeight(_aimRig, currentValue));
            }

            _lastValue = currentValue;
        }

        private IEnumerator ChangeWeight(Rig rig, float targetValue)
        {
            float step = -(rig.weight - targetValue) * Time.fixedDeltaTime * aimRigWeightChangeCoeff;

            while (Mathf.Abs(rig.weight - targetValue) > Appromixation)
            {
                rig.weight += step;
                yield return new WaitForFixedUpdate();
            }
        }

        private void HandleWalkState(float mag)
        {
            if (_inputData.LeftShift || _inputData.VerticalAxisRaw != 0 || !CanAnimate || _isStandUp)
                return;

            switch (mag)
            {
                case > VelocityApproximation:
                    _playerStateMachine.SetState<WalkForwardState>();
                    break;
                case < -VelocityApproximation:
                    _playerStateMachine.SetState<WalkBackwardState>();
                    break;
            }
        }

        private void HandleRunStates(float mag)
        {
            if (mag < Appromixation || !_inputData.LeftShift || !CanAnimate || _isStandUp)
                return;

            switch (_inputData.VerticalAxisRaw)
            {
                case > 0:
                    _playerStateMachine.SetState<RunForwardState>();
                    break;
                case < 0:
                    _playerStateMachine.SetState<RunBackwardState>();
                    break;
            }
        }

        private void HandleHorizontalState(float mag)
        {
            if (mag < VelocityApproximation || !CanAnimate || _isStandUp)
                return;

            switch (_inputData.HorizontalAxisRaw)
            {
                case < 0:
                    _playerStateMachine.SetState<WalkLeftState>();
                    break;
                case > 0:
                    _playerStateMachine.SetState<WalkRightState>();
                    break;
            }
        }

        private void HandleIdleState(float magnitude)
        {
            if (!CanAnimate || _isStandUp)
                return;

            float mag = _ragdoll.linearVelocity.magnitude;
            if (mag is < VelocityApproximation and > - VelocityApproximation)
            {
                _playerStateMachine.SetState<IdleState>();
            }
        }

        public void StartStandUp()
        {
            StartCoroutine(Count());
        }
        
        private IEnumerator Count()
        {
            _counter = 0;
            _isStandUp = true;
            while (_counter < TimeToWait)
            {
                _counter += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            _isStandUp = false;
        }
    }
}