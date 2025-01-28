using System.Collections.Generic;
using UnityEngine;

namespace Shadow_Dominion.Zombie
{
    public interface IZombieTarget
    {
        public IEnumerable<Transform> Position { get; set; }
    }
}