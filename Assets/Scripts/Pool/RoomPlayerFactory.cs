using Mirror;
using Zenject;

namespace Shadow_Dominion
{
    public class RoomPlayerFactory : Factory<NetworkRoomPlayer>
    {
        public RoomPlayerFactory(IInstantiator instantiator, NetworkRoomPlayer prefab) : base(instantiator, prefab)
        {
        }
    }
}