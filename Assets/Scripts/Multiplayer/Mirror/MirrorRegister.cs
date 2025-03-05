using Mirror;
using Multiplayer.Structs;

namespace Shadow_Dominion
{
    public class MirrorRegister
    {
        public void Register()
        {
            NetworkServer.RegisterHandler<PlayerStateMessage>(MirrorPlayerStateSyncer.Instance.CmdSetState);
            NetworkServer.RegisterHandler<PositionMessage>(MirrorPlayerSpawner.Instance.OnCreateCharacter);
            //NetworkServer.RegisterHandler<RoomPlayerSpawnMessage>(MirrorPlayerSpawner.Instance.OnCreateRoomPlayer);
        }

        public void UnRegister()
        {
            NetworkServer.UnregisterHandler<PlayerStateMessage>();
            NetworkServer.UnregisterHandler<PositionMessage>();
            //NetworkServer.UnregisterHandler<RoomPlayerSpawnMessage>();
        }
    }
}