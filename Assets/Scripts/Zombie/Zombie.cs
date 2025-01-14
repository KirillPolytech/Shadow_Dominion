using UnityEngine;

namespace Shadow_Dominion.Zombie
{
    public class Zombie : MonoBehaviour
    {
        [SerializeField] private float force = 10;

        public void Construct()
        {
        }

        public void Disable(BoneController[] boneController, Vector3 dir)
        {
            for (int i = 0; i < boneController.Length; i++)
            {
                boneController[i].IsPositionApplying(false);
                boneController[i].IsRotationApplying(false);
                boneController[i].AddForce(dir * force);
            }
        }
    }
}