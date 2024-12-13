using Mirror;
using UnityEngine.UI;

public class StartButton : Button
{
    protected override void Awake()
    {
        base.Awake();

        onClick.AddListener(() => NetworkManager.singleton.ServerChangeScene("Level"));
    }
}