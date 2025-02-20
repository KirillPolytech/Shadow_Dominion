using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Shadow_Dominion.Zombie;
using UnityEngine;

namespace Shadow_Dominion.Main
{
    public class Player : Humanoid, IZombieTarget
    {
        public Transform Position { get; set; }
        
        public PlayerStateMachine PlayerStateMachine;
        
        private Rigidbody _rigidbody;

        public void Construct(Transform t, PlayerStateMachine playerStateMachine)
        {
            Position = t;
            _rigidbody = t.GetComponent<Rigidbody>();
            PlayerStateMachine = playerStateMachine;
        }

        public void IsKinematic(bool iskinematic)
        {
            _rigidbody.isKinematic = iskinematic;
        }
        
        public void SetPositionAndRotation(Vector3 pos, Quaternion rot)
        {
            _rigidbody.position = pos;
            _rigidbody.rotation = rot;
        }
    }
}