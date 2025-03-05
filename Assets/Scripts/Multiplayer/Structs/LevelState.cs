using System;
using Mirror;

namespace Multiplayer.Structs
{
    [Serializable]
    public struct LevelState : NetworkMessage
    {
        public string StateName;
        
        public LevelState(string stateName)
        {
            StateName = stateName;
        }
    }
}