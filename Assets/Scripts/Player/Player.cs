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
        
        private void Awake()
        {
            Position = transform;
        }

        public void Construct(
            RigBuilder rootRig, 
            PlayerMovement playerMovement, 
            PlayerAnimation playerAnimation,
            BoneController[] copyTo)
        {
            playerStateMachine = new PlayerStateMachine(playerMovement, playerAnimation, rootRig, copyTo);
        }
    }
}