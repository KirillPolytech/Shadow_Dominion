using UnityEngine;

namespace Shadow_Dominion
{
    public class SpawnPointSyncer : MirrorSingleton<SpawnPointSyncer>
    {
        [SerializeField] private Transform[] spawnPoints;

        private Vector3 Center => Vector3.zero;

        public Quaternion CalculateRotation(Vector3 position)
        {
            Vector3 dir = Center - position;
            dir.y = 0;
            return Quaternion.LookRotation(dir);
        }

        private new void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);
        }

        public Vector3 GetFreePosition(int ind) => spawnPoints[ind].position;
    }
}