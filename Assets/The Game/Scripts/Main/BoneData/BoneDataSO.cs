using Shadow_Dominion;
using UnityEngine;

[CreateAssetMenu(fileName = "BoneData", menuName = PathStorage.ScriptableObjectMenu + "/BoneData", order = 0)]
public class BoneDataSO : ScriptableObject
{
    public BoneData[] BoneData;
}