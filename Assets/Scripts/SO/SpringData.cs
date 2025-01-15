using UnityEngine;

namespace Shadow_Dominion
{
    [CreateAssetMenu(fileName = "SpringData", menuName = PathStorage.ScriptableObjectMenu + "/SpringData")]
    public class SpringData : ScriptableObject
    {
        public float Rate = 45;
        public float DeltaTime = 100;
        public float DeltaMax = 100;
        public int SpringDelta = 50;
    }
}