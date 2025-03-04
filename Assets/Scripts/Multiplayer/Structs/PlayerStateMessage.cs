using Mirror;

namespace Multiplayer.Structs
{
    public struct PlayerStateMessage : NetworkMessage
    {
        public string StateName;
        
        public PlayerStateMessage(string stateName)
        {
            StateName = stateName;
        }
    }
}