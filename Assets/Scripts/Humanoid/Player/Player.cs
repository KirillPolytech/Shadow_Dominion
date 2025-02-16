using System.Collections.Generic;
using Shadow_Dominion.InputSystem;
using Shadow_Dominion.Player;
using Shadow_Dominion.Player.StateMachine;
using Shadow_Dominion.Zombie;
using UnityEngine;
using UnityEngine.Animations.Rigging;

namespace Shadow_Dominion.Main
{
    public class Player : Humanoid, IZombieTarget
    {
        public IEnumerable<Transform> Position { get; set; }
        
        private Rigidbody _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            Position = new []{transform};
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