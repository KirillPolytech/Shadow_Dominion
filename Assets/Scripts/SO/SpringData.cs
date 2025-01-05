using UnityEngine;

namespace HellBeavers
{
    [CreateAssetMenu(fileName = "SpringData", menuName = "HellBeaversData/SpringData")]
    public class SpringData : ScriptableObject
    {
        public float Rate = 20;
        public float DeltaTime = 100;
        public float DeltaMax = 100;
        public int SpringDelta = 50;
    }
}