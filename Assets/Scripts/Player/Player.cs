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
        
        public void Disable(BoneController[] boneController, Vector3 dir)
        {
            for (int i = 0; i < boneController.Length; i++)
            {
                boneController[i].IsPositionApplying(false);
                boneController[i].IsRotationApplying(false);
                boneController[i].IsFreezeed(false);
                boneController[i].AddForce(dir);
            }
        }
    }
}