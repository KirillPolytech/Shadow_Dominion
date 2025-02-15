using UnityEngine;

[CreateAssetMenu(fileName = "ZombieSettings", menuName = PathStorage.ScriptableObjectMenu + "/ZombieSettings")]
public class ZombieSettings : ScriptableObject
{
    public int speed = 2;
    public int acceleration = 1;
}