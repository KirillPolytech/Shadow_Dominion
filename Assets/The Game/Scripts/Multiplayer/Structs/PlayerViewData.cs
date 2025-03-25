using System;

namespace Shadow_Dominion
{
    [Serializable]
    public struct PlayerViewData
    {
        public string Address;
        public bool IsReady;
        public ushort Kills;

        public PlayerViewData(string address, bool isReady, ushort kills)
        {
            Address = address;
            IsReady = isReady;
            Kills = kills;
        }
    }
}