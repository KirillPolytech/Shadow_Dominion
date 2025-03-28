using Mirror;
using Multiplayer.Structs;
using UnityEngine;

namespace Shadow_Dominion
{
    public class SpawnPointSyncer : MirrorSingleton<SpawnPointSyncer>
    {
        private readonly SyncList<PositionMessage> _positionMessages = new();
        private Vector3 Center => Vector3.zero;

        public PositionMessage[] positionMessage =
        {
            new(new Vector3(22, 0, 0), true),
            new(new Vector3(-22, 0, 0), true),
            new(new Vector3(0, 0, 15), true),
            new(new Vector3(0, 0, -15), true)
        };
        
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
            
            if (_positionMessages.Count != 0)
                return;
            
            _positionMessages.AddRange(positionMessage);
        }

        public void Reset()
        {
            if (!isServer)
                return;
            
            _positionMessages.Clear();
            _positionMessages.AddRange(positionMessage);
        }

        public PositionMessage GetFreePosition(PlayerViewData playerViewData)=> _positionMessages[playerViewData.ID];
        public PositionMessage GetFreePosition(int ind)=> _positionMessages[ind];
    }
}