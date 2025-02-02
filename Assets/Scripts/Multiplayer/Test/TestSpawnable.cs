using Mirror;
using UnityEngine;

public class TestSpawnable : NetworkBehaviour
{
    private readonly SyncList<int> namesList = new SyncList<int>();
    
    public void OnEnable()
    {
        Debug.Log("TestSpawnable spawned!");
        
        Register();
    
        namesList.OnChange += (arg1, arg2, arg3) => 
            { Debug.Log($"namesList.OnChange {arg1} {arg2} {arg3}"); };
    }

    private void Register()
    {
        NetworkClient.RegisterHandler<ScoreMessage2>(OnScore);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SendScore(79);

            namesList.Add( 987);
        }
    }

    public void SendScore(int score)
    {
        ScoreMessage2 msg = new ScoreMessage2 { health = score };
        NetworkServer.SendToAll(msg);
    }

    public void OnScore(ScoreMessage2 msg)
    {
        Debug.Log("OnScoreMessage " + msg.health);
    }
}

public struct ScoreMessage2 : NetworkMessage
{
    public int health;
}