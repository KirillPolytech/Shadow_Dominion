using System;

namespace Shadow_Dominion
{
    [Serializable]
    public struct PlayerViewData : IEquatable<PlayerViewData>
    {
        public string Address;
        public bool IsReady;
        public ushort Kills;
        public bool IsLocalPlayer;

        public PlayerViewData(string address, bool isReady, ushort kills, bool isLocalPlayer)
        {
            Address = address;
            IsReady = isReady;
            Kills = kills;
            IsLocalPlayer = isLocalPlayer;
        }

        public bool Equals(PlayerViewData other)
        {
            return Address == other.Address && IsReady == other.IsReady && Kills == other.Kills && IsLocalPlayer == other.IsLocalPlayer;
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerViewData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Address, IsReady, Kills, IsLocalPlayer);
        }
    }
}