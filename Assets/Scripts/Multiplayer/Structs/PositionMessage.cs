using Mirror;
using UnityEngine;

namespace Multiplayer.Structs
{
    public struct PositionMessage : NetworkMessage
    {
        public Vector3 pos;
        public bool isFree;
    }
}