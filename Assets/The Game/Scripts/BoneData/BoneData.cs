using System;
using UnityEngine;

namespace Shadow_Dominion
{
    [Serializable]
    public struct BoneData
    {
        public string Name;
        public HumanBodyBones humanBodyBone;

        public int angularYLimit;
        public int angularZLimit;

        public int lowAngularXLimit;
        public int highAngularXLimit;
    }
}