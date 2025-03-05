using System;
using Mirror;
using UnityEngine;

namespace Multiplayer.Structs
{
    [Serializable]
    public struct PositionMessage : NetworkMessage
    {
        public Vector3 pos;
        public bool isFree;
    }
}