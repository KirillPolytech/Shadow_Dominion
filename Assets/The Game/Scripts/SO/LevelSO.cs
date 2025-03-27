using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = PathStorage.ScriptableObjectMenu + "/LevelSO")]
public class LevelSO : ScriptableObject
{
    public uint InitializeWaitTime = 30;
    public uint Rounds = 5;
}