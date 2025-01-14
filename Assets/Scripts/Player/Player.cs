using Shadow_Dominion.Zombie;
using UnityEngine;

namespace Shadow_Dominion.Main
{
    public class Player : MonoBehaviour, IZombieTarget
    {
        public Transform Position { get; set; }

        private void Awake()
        {
            Position = transform;
        }
    }
}