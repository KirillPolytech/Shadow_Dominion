using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Shadow_Dominion.Zombie;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Main
{
    public class Player : MonoBehaviour, IZombieTarget
    {
        public PlayerStateMachine playerStateMachine;
        public Transform Position { get; set; }

        private MonoInputHandler _monoInputHandler;
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            Position = transform;
        }

        private void HandleInput(InputData inputData)
        {
            if (!inputData.F_Down)
                return;

            playerStateMachine.SetState<StandUpState>();
        }

        public void Construct(
            Transform ragdollRoot,
            RigBuilder rootRig,
            PlayerMovement playerMovement,
            PlayerAnimation playerAnimation,
            BoneController[] copyTo,
            MonoInputHandler monoInputHandler)
        {
            playerStateMachine =
                new PlayerStateMachine(this, playerMovement, ragdollRoot, playerAnimation, rootRig, copyTo);

            _monoInputHandler = monoInputHandler;

            _monoInputHandler.OnInputUpdate += HandleInput;
        }

        public void SetPositionAndRotation(Vector3 pos, Vector3 rotDir)
        {
            _rigidbody.position = new Vector3(pos.x, _rigidbody.position.y, pos.z);

            rotDir = new Vector3(rotDir.x, 0, rotDir.z);
            _rigidbody.rotation = Quaternion.LookRotation(rotDir);
        }

        private void OnDestroy()
        {
            _monoInputHandler.OnInputUpdate -= HandleInput;
        }
    }
}