using System.Collections;
using Shadow_Dominion.AnimStateMachine;
using Shadow_Dominion.InputSystem;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Player
{
    public class PlayerAnimation
    {
        public AnimationStateMachine AnimationStateMachine { get; private set; }
        
        private Rig _aimRig;
        private Coroutine _coroutine;
        private CoroutineExecuter _coroutineExecuter;
        private PlayerSettings _playerSettings;
        
        private int _lastValue;
        private float _counter;

        public void Construct(
            AnimationStateMachine animationStateMachine,
            Rig aimRig,
            CoroutineExecuter coroutineExecuter,
            PlayerSettings playerSettings)
        {
            _aimRig = aimRig;
            _coroutineExecuter = coroutineExecuter;
            AnimationStateMachine = animationStateMachine;
            _playerSettings = playerSettings;
        }

        public void HandleAimRig(InputData inputData)
        {
            int currentValue = inputData.RightMouseButton ? 1 : 0;

            if (_lastValue != currentValue)
            {
                if (_coroutine != null)
                    _coroutineExecuter.StopCoroutine(_coroutine);

                _coroutine = _coroutineExecuter.StartCoroutine(ChangeWeight(_aimRig, currentValue));
            }

            _lastValue = currentValue;
        }

        private IEnumerator ChangeWeight(Rig rig, float targetValue)
        {
            float step = -(rig.weight - targetValue) * Time.fixedDeltaTime * _playerSettings.aimRigWeightChange;

            while (Mathf.Abs(rig.weight - targetValue) > _playerSettings.Approximation)
            {
                rig.weight += step;
                yield return new WaitForFixedUpdate();
            }
        }
    }
}