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
            NetworkRoomPlayer t = Object.Instantiate(_prefab);
            t.name = t.GetType().ToString();
            return t;
        }
    }
}