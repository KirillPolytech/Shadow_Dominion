using System;
using Mirror;
using UnityEngine;

namespace Multiplayer.Structs
{
    [Serializable]
    public struct PositionMessage : NetworkMessage
    {
        public Vector3 Position;
        public bool IsFree;

        public PositionMessage(Vector3 position, bool isFree)
        {
            Position = position;
            IsFree = isFree;
        }
    }
}