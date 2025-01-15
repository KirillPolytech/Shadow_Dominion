using UnityEngine;

namespace Shadow_Dominion.Zombie
{
    public class Zombie : MonoBehaviour
    {
        [SerializeField] private Rigidbody zombie_Ragdoll;
        
        public void Disable(BoneController[] boneController, Vector3 dir)
        {
            zombie_Ragdoll.constraints = RigidbodyConstraints.None;
            
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