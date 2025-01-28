using System;
using System.Collections.Generic;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Shadow_Dominion.Zombie;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Main
{
    public class Player : Humanoid, IZombieTarget
    {
        public PlayerStateMachine playerStateMachine;
        public IEnumerable<Transform> Position { get; set; }

        private MonoInputHandler _monoInputHandler;
        private Rigidbody _rigidbody;
        private Transform _ragdollRoot;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            Position = new []{transform};
        }

        public void Construct(
            Transform ragdollRoot,
            RigBuilder rootRig,
            PlayerMovement playerMovement,
            PlayerAnimation playerAnimation,
            CameraLook cameraLook,
            BoneController[] copyTo,
            MonoInputHandler monoInputHandler)
        {
            playerStateMachine =
                new PlayerStateMachine(this, playerMovement, cameraLook, ragdollRoot, playerAnimation, rootRig, copyTo);

            _monoInputHandler = monoInputHandler;
            _ragdollRoot = ragdollRoot;

            _monoInputHandler.OnInputUpdate += HandleInput;
        }
        
        private void HandleInput(InputData inputData)
        {
            if (!inputData.F_Down)
                return;

            if (Vector3.Dot(_ragdollRoot.forward, Vector3.up) > 0)
                playerStateMachine.SetState<StandUpFaceUpState>();
            else
                playerStateMachine.SetState<StandUpFaceDownState>();
        }

        public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
        {
            _rigidbody.position = pos;
            _rigidbody.rotation = rot;
        }

        private void OnDestroy()
        {
            _monoInputHandler.OnInputUpdate -= HandleInput;
        }
    }
}