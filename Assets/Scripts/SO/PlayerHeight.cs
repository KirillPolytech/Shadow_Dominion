using UnityEngine;

namespace HellBeavers.Player
{
    [CreateAssetMenu(fileName = "PlayerHeight", menuName = "HellBeaversData/PlayerHeight")]
    public class PlayerHeightData : ScriptableObject
    {
        public float distance = 15;
        public float targetHeight = 0.7f;
        public LayerMask layerMask;
    }
}