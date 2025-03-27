using System;

namespace Shadow_Dominion
{
    [Serializable]
    public struct PlayerViewData : IEquatable<PlayerViewData>
    {
        public string Nick;
        public bool IsReady;
        public ushort Kills;

        public PlayerViewData(string nick, bool isReady, ushort kills)
        {
            Nick = nick;
            IsReady = isReady;
            Kills = kills;
        }

        public bool Equals(PlayerViewData other)
        {
            return Nick == other.Nick && IsReady == other.IsReady && Kills == other.Kills;
        }

        public override bool Equals(object obj)
        {
            return obj is PlayerViewData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Nick, IsReady, Kills);
        }
    }
}