using System;

namespace Shadow_Dominion
{
    [Serializable]
    public struct PlayerViewData
    {
        public string Address;
        public bool IsReady;

        public PlayerViewData(string address, bool isReady)
        {
            Address = address;
            IsReady = isReady;
        }
    }
}