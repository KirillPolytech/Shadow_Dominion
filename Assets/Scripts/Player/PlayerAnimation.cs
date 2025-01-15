using System.Collections;
using Mirror;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player
{
    public class PlayerAnimation : NetworkBehaviour
    {
        public AnimationStateMachine AnimationStateMachine { get; private set; }

        public Animator Animator { get; private set; }

        private MonoInputHandler _inputHandler;
        private Rig _aimRig;
        private Coroutine _coroutine;
        private int _lastValue;

        public void Construct(
            Animator animator,
            MonoInputHandler inputHandler,
            Rig aimRig)
        {
            Animator = animator;
            _inputHandler = inputHandler;
            _aimRig = aimRig;
        }

        private void Start()
        {
            AnimationStateMachine = new AnimationStateMachine(Animator);

            if (!isLocalPlayer)
                return;

            AnimationStateMachine.SetState<IdleState>();

            _inputHandler.OnInputUpdate += HandleHorizontalState;
            _inputHandler.OnInputUpdate += HandleWalkState;
            _inputHandler.OnInputUpdate += HandleIdleState;
            _inputHandler.OnInputUpdate += HandleRunStates;
            _inputHandler.OnInputUpdate += HandleStandUpState;
            _inputHandler.OnInputUpdate += HandleAimRig;
        }

        private void HandleAimRig(InputData inputData)
        {
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
            float step = -(rig.weight - targetValue) * Time.fixedDeltaTime * 5;

            while (Mathf.Abs(rig.weight - targetValue) > 0.01f)
            {
                rig.weight += step;
                yield return new WaitForFixedUpdate();
            }
        }

        private void HandleWalkState(InputData inputData)
        {
            //Debug.Log($"IsPressedShift: {inputData.LeftShift}");

            if (inputData.LeftShift)
                return;

            switch (inputData.VerticalAxisRaw)
            {
                case > 0:
                    AnimationStateMachine.SetState<WalkForwardState>();
                    break;
                case < 0:
                    AnimationStateMachine.SetState<WalkBackwardState>();
                    break;
            }
        }

        private void HandleRunStates(InputData inputData)
        {
            if (!inputData.LeftShift)
                return;

            switch (inputData.VerticalAxisRaw)
            {
                case > 0:
                    AnimationStateMachine.SetState<RunForwardState>();
                    break;
                case < 0:
                    AnimationStateMachine.SetState<WalkBackwardState>();
                    break;
            }
        }

        private void HandleHorizontalState(InputData inputData)
        {
            switch (inputData.HorizontalAxisRaw)
            {
                case < 0:
                    AnimationStateMachine.SetState<WalkLeftState>();
                    break;
                case > 0:
                    AnimationStateMachine.SetState<WalkRightState>();
                    break;
            }
        }

        private void HandleIdleState(InputData inputData)
        {
            if (inputData is { HorizontalAxisRaw: 0, VerticalAxisRaw: 0 })
            {
                AnimationStateMachine.SetState<IdleState>();
            }
        }

        private void HandleStandUpState(InputData inputData)
        {
            if (!inputData.E)
                return;

            //AnimationStateMachine.SetState<StandupState>();
        }

        public void OnDestroy()
        {
            if (isLocalPlayer)
                return;

            _inputHandler.OnInputUpdate -= HandleHorizontalState;
            _inputHandler.OnInputUpdate -= HandleWalkState;
            _inputHandler.OnInputUpdate -= HandleIdleState;
            _inputHandler.OnInputUpdate -= HandleRunStates;
            _inputHandler.OnInputUpdate -= HandleStandUpState;
            _inputHandler.OnInputUpdate -= HandleAimRig;
        }
    }
}