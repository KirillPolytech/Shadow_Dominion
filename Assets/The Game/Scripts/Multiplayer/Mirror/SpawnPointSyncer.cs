using System;
using System.Linq;
using Mirror;
using Multiplayer.Structs;
using UnityEngine;

namespace Shadow_Dominion
{
    public class SpawnPointSyncer : MirrorSingleton<SpawnPointSyncer>
    {
        private readonly SyncList<PositionMessage> _positionMessages = new();
        private readonly SyncList<bool> _positionFree = new();
        private readonly bool[] _frees = {true,true,true,true };
        private Vector3 Center => Vector3.zero;

        public PositionMessage[] positionMessage =
        {
            new(new Vector3(55, 0, 0), true),
            new(new Vector3(-55, 0, 0), true),
            new(new Vector3(0, 0, 55), true),
            new(new Vector3(0, 0, -55), true)
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
            
            _positionFree.AddRange(_frees);
        }

        public void Reset()
        {
            if (!isServer)
                return;
            
            _positionMessages.Clear();
            _positionMessages.AddRange(positionMessage);
        }

        public PositionMessage GetFreePosition()
        {
            for (int i = 0; i < _positionFree.Count; i++)
            {
                if (!_positionFree.ElementAt(i)) 
                    continue;
                
                PositionMessage temp = _positionMessages.ElementAt(i);

                temp.IsFree = false;

                UpdateSyncList(i);

                return temp;
            }

            throw new Exception("Can't find free position.");
        }

        [Command(requiresAuthority = false)]
        private void UpdateSyncList(int ind)
        {
            _positionFree[ind] = false;
            
            PositionMessage temp = _positionMessages.ElementAt(ind);

            temp.IsFree = false;

            _positionMessages.Remove(temp);
            _positionMessages.Add(temp);
            
            Debug.Log("[Server] PositionSyncer updated.");
        }
    }
}