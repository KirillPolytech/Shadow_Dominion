using UnityEngine;

[CreateAssetMenu(fileName = "RoomSettings", menuName = "HellBeaversData/RoomSettings")]
public class RoomSettings : ScriptableObject
{
    public int MaxPlayers = 4;
    public bool IsVisible = true;
}