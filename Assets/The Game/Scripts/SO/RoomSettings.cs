using UnityEngine;

[CreateAssetMenu(fileName = "RoomSettings", menuName = PathStorage.ScriptableObjectMenu + "/RoomSettings")]
public class RoomSettings : ScriptableObject
{
    public string mainLevel = "Level";
    public string offlineLevel = "Menu";
    
    public int MaxPlayers = 4;
    public bool IsVisible = true;
}