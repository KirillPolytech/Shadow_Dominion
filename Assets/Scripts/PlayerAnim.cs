using Mirror;
using UnityEngine;

namespace HellBeavers.Player
{
    public class PlayerAnim : NetworkBehaviour
    {
        public AnimationStateMachine AnimationStateMachine { get; private set; }

        public Animator Animator { get; private set; }

        private bool _isRun;
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
            _inputHandler.OnInputUpdate += HandleVerticalState;
            _inputHandler.OnInputUpdate += HandleIdleState;
            _inputHandler.OnInputUpdate += HandleRunStates;
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

        private void HandleVerticalState(InputData inputData)
        {
            _isRun = inputData.LeftShift;

            switch (inputData.VerticalAxisRaw)
            {
                case > 0:
                    if (_isRun)
                        break;
                    AnimationStateMachine.SetState<WalkForwardState>();
                    break;
                case < 0:
                    if (_isRun)
                        break;
                    AnimationStateMachine.SetState<WalkBackwardState>();
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

        private void HandleRunStates(InputData inputData)
        {
            switch (_isRun)
            {
                case var _ when inputData.VerticalAxisRaw > 0 && _isRun:
                    AnimationStateMachine.SetState<RunForwardState>();
                    break;
                case var _ when inputData.VerticalAxisRaw < 0 && _isRun:
                    AnimationStateMachine.SetState<WalkBackwardState>();
                    break;
            }
        }

        public void OnDestroy()
        {            
            if (isLocalPlayer)
                return;
            
            _inputHandler.OnInputUpdate -= HandleHorizontalState;
            _inputHandler.OnInputUpdate -= HandleVerticalState;
            _inputHandler.OnInputUpdate -= HandleIdleState;
            _inputHandler.OnInputUpdate -= HandleRunStates;
        }
    }
}