using UnityEngine;

namespace Shadow_Dominion
{
    [CreateAssetMenu(fileName = "WeaponSO", menuName = PathStorage.ScriptableObjectMenu + "/WeaponSO")]
    public class WeaponSO : ScriptableObject
    {
        public float FireDelay = 0.1f;
        public float Damage = 100;
        public float RotationSpeed = 15;
        public float Limit = 45;
    }
}