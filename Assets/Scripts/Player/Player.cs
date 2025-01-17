using Shadow_Dominion.Player;
using Shadow_Dominion.Zombie;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Main
{
    public class Player : MonoBehaviour, IZombieTarget
    {
        public Transform Position { get; set; }

        private RigBuilder _rootRig;
        private PlayerMovement _playerMovement;
        private PlayerAnimation _playerAnimation;

        private void Awake()
        {
            Position = transform;
        }

        public void Construct(RigBuilder rootRig, PlayerMovement playerMovement, PlayerAnimation playerAnimation)
        {
            _rootRig = rootRig;
            _playerMovement = playerMovement;
            _playerAnimation = playerAnimation;
        }

        public void SetRagdollState(bool isEnabled, BoneController[] boneController, Vector3 dir)
        {
            _rootRig.enabled = isEnabled;
            _playerMovement.CanMove = isEnabled;
            _playerAnimation.CanAnimate = isEnabled;

            for (int i = 0; i < boneController.Length; i++)
            {
                boneController[i].IsPositionApplying(isEnabled);
                boneController[i].IsRotationApplying(isEnabled);
                boneController[i].AddForce(dir);
            }
        }
    }
}