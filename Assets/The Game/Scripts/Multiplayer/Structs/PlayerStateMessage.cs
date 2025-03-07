using System;
using Mirror;

namespace Multiplayer.Structs
{
    [Serializable]
    public struct PlayerStateMessage : NetworkMessage
    {
        public string StateName;
        
        public PlayerStateMessage(string stateName)
        {
            StateName = stateName;
        }
    }
}