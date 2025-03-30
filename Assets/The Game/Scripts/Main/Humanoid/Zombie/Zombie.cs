using Shadow_Dominion.Player;
using UnityEngine;

namespace Shadow_Dominion.Zombie
{
    public class Zombie : Humanoid
    {
        [SerializeField] private Rigidbody zombie_Ragdoll;

        public void Disable(BoneController[] boneController, Vector3 dir)
        {
            zombie_Ragdoll.constraints = RigidbodyConstraints.None;
            
            for (int i = 0; i < boneController.Length; i++)
            {
                boneController[i].IsPositionApplying(false);
                boneController[i].IsRotationApplying(false);
                boneController[i].AddForce(dir);
            }
            
            //boneController.IsPositionApplying(false);
            //boneController.IsRotationApplying(false);
            //boneController.AddForce(dir);
        }
    }
}