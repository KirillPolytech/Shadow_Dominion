using UnityEngine;

[CreateAssetMenu(fileName = "TextSO", menuName = PathStorage.ScriptableObjectMenu + "/TextSO")]
public class TextSO : ScriptableObject
{
    public int NickLength = 10;
    public string ViewReadyState = "Ready";
    public string ViewNotReadyState = "Not ready";
}