using Mirror;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class RoomPlayerFactory : Factory<NetworkRoomPlayer>
    {
        public RoomPlayerFactory(IInstantiator instantiator, NetworkRoomPlayer prefab) : base(instantiator, prefab)
        {
        }

        public override NetworkRoomPlayer Create()
        {
            NetworkRoomPlayer t = _instantiator
                .InstantiatePrefab(_prefab, Vector3.zero, Quaternion.identity, null).GetComponent<NetworkRoomPlayer>();
            
            t.transform.parent = null;
            Object.DontDestroyOnLoad(t.gameObject);
            t.name = t.GetType().ToString();
            return t;
        }
    }
}