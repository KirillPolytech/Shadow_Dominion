using Mirror;
using UnityEngine;

namespace HellBeavers.Player
{
    public class PlayerAnim : NetworkBehaviour
    {
        public AnimationStateMachine AnimationStateMachine { get; private set; }

        public Animator Animator { get; private set; }

        private MonoInputHandler _inputHandler;

        public void Construct(
            Animator animator,
            MonoInputHandler inputHandler)
        {
            Animator = animator;
            _inputHandler = inputHandler;
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
        }

        private void HandleWalkState(InputData inputData)
        {
            Debug.Log($"IsPressedShift: {inputData.LeftShift}");
            
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
        }
    }
}