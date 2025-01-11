using System;

namespace Shadow_Dominion
{
    [Serializable]
    public struct BoneData
    {
        public string Name;

        public int angularYLimit;
        public int angularZLimit;

        public int lowAngularXLimit;
        public int highAngularXLimit;
    }
}