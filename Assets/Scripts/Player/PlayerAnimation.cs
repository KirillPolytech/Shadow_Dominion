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

        public bool CanAnimate { get; set; } = true;

        public AnimationStateMachine AnimationStateMachine { get; private set; }

        public Animator Animator { get; private set; }

        [Range(0, 10f)]
        [SerializeField]
        private float aimRigWeightChangeCoeff = 3.5f;

        private MonoInputHandler _inputHandler;
        private PlayerStateMachine _playerStateMachine;
        private Rig _aimRig;

        private Coroutine _coroutine;
        private int _lastValue;
        
        private const float TimeToWait = 7;
        private float _counter;
        private bool _isStandUp;

        public void Construct(
            Animator animator,
            MonoInputHandler inputHandler,
            Rig aimRig,
            PlayerStateMachine playerStateMachine)
        {
            Animator = animator;
            _inputHandler = inputHandler;
            _aimRig = aimRig;
            _playerStateMachine = playerStateMachine;

            AnimationStateMachine = new AnimationStateMachine(Animator);
        }

        private void Start()
        {
            if (!isLocalPlayer)
                return;

            _inputHandler.OnInputUpdate += HandleHorizontalState;
            _inputHandler.OnInputUpdate += HandleWalkState;
            _inputHandler.OnInputUpdate += HandleIdleState;
            _inputHandler.OnInputUpdate += HandleRunStates;
            _inputHandler.OnInputUpdate += HandleAimRig;
        }

        private void HandleAimRig(InputData inputData)
        {
            if (!CanAnimate || _isStandUp)
                return;

            int currentValue = inputData.RightMouseButton ? 1 : 0;

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

        private void HandleWalkState(InputData inputData)
        {
            if (inputData.LeftShift || !CanAnimate || _isStandUp)
                return;

            switch (inputData.VerticalAxisRaw)
            {
                case > 0:
                    _playerStateMachine.SetState<WalkForwardState>();
                    break;
                case < 0:
                    _playerStateMachine.SetState<WalkBackwardState>();
                    break;
            }
        }

        private void HandleRunStates(InputData inputData)
        {
            if (!inputData.LeftShift || !CanAnimate || _isStandUp)
                return;

            switch (inputData.VerticalAxisRaw)
            {
                case > 0:
                    _playerStateMachine.SetState<RunForwardState>();
                    break;
                case < 0:
                    _playerStateMachine.SetState<RunBackwardState>();
                    break;
            }
        }

        private void HandleHorizontalState(InputData inputData)
        {
            if (!CanAnimate || _isStandUp)
                return;

            switch (inputData.HorizontalAxisRaw)
            {
                case < 0:
                    _playerStateMachine.SetState<WalkLeftState>();
                    break;
                case > 0:
                    _playerStateMachine.SetState<WalkRightState>();
                    break;
            }
        }

        private void HandleIdleState(InputData inputData)
        {
            if (!CanAnimate || _isStandUp)
                return;

            if (inputData is { HorizontalAxisRaw: 0, VerticalAxisRaw: 0 })
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

        public void OnDestroy()
        {
            if (isLocalPlayer)
                return;

            _inputHandler.OnInputUpdate -= HandleHorizontalState;
            _inputHandler.OnInputUpdate -= HandleWalkState;
            _inputHandler.OnInputUpdate -= HandleIdleState;
            _inputHandler.OnInputUpdate -= HandleRunStates;
            _inputHandler.OnInputUpdate -= HandleAimRig;
        }
    }
}