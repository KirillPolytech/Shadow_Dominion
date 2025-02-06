using System.Collections;
using Shadow_Dominion.Player.StateMachine;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player
{
    public class PlayerAnimation
    {
        private const float Appromixation = 0.01f;
        private const float VelocityApproximation = 1.5f;

        public bool CanAnimate { get; set; } = true;

        public AnimationStateMachine AnimationStateMachine { get; private set; }

        public Animator Animator { get; private set; }

        [Range(0, 10f)]
        [SerializeField]
        private float aimRigWeightChangeCoeff = 3.5f;

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
            Rig aimRig,
            Rigidbody ragdoll,
            PlayerStateMachine playerStateMachine)
        {
            Animator = animator;
            _aimRig = aimRig;
            _playerStateMachine = playerStateMachine;
            _ragdoll =ragdoll;

            AnimationStateMachine = new AnimationStateMachine(Animator);
        }

        private void HandleAimRig()
        {
            if (!CanAnimate || _isStandUp)
                return;

            int currentValue = _inputData.RightMouseButton ? 1 : 0;

            if (_lastValue != currentValue)
            {
                //if (_coroutine != null)
                    //StopCoroutine(_coroutine);

                //_coroutine = StartCoroutine(ChangeWeight(_aimRig, currentValue));
            }

            _lastValue = currentValue;
        }

        public void HandleIdle()
        {
            _playerStateMachine.SetState<IdleState>();
        }
        
        public void HandleVerticalWalk(float verticalAxisRaw)
        {
            switch (verticalAxisRaw)
            {
                case > 0:
                    _playerStateMachine.SetState<WalkForwardState>();
                    break;
                case < 0:
                    _playerStateMachine.SetState<WalkBackwardState>();
                    break;
            }
        }

        public void HandleRun(float verticalAxisRaw)
        {
            switch (verticalAxisRaw)
            {
                case > 0:
                    _playerStateMachine.SetState<RunForwardState>();
                    break;
                case < 0:
                    _playerStateMachine.SetState<RunBackwardState>();
                    break;
            }
        }

        public void HandleHorizontalState(float horizontalAxisRaw)
        {
            switch (horizontalAxisRaw)
            {
                case < 0:
                    _playerStateMachine.SetState<WalkLeftState>();
                    break;
                case > 0:
                    _playerStateMachine.SetState<WalkRightState>();
                    break;
            }
        }
        
        public void HandleDiagonal(float x, float y)
        {
            switch (y)
            {
                case > 0 when x > 0:
                    _playerStateMachine.SetState<WalkDiagonallyRightState>();
                    break;
                case > 0 when x < 0:
                    _playerStateMachine.SetState<WalkDiagonallyLeftState>();
                    break;
            }
        }

        public void StartStandUp()
        {
            //StartCoroutine(Count());
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