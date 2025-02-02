using Mirror;
using UnityEngine;

public class TestBehaviour : NetworkBehaviour
{
    private readonly SyncList<int> namesList = new SyncList<int>();
    
    public void OnEnable()
    {
        Debug.Log("TestBehaviour spawned!");
        
        //Register();

        namesList.OnChange += (arg1, arg2, arg3) => 
            { Debug.Log($"namesList.OnChange {arg1} {arg2} {arg3}"); };
    }

    private void Register()
    {
        //NetworkClient.RegisterHandler<ScoreMessage>(OnScore);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            //SendScore(250);

            namesList.Add( 512);
        }
    }

    // public void SendScore(int score)
    // {
    //     ScoreMessage msg = new ScoreMessage { health = score };
    //     NetworkServer.SendToAll(msg);
    // }
    //
    // public void OnScore(ScoreMessage msg)
    // {
    //     Debug.Log("OnScoreMessage " + msg.health);
    // }
}

public struct ScoreMessage : NetworkMessage
{
    public int health;
}