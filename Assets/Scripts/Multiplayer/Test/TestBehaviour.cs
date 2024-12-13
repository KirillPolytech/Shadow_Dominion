using Mirror;
using UnityEngine;

public class TestBehaviour : NetworkBehaviour
{
    public readonly SyncList<Vector3> namesList = new SyncList<Vector3>();

    public TestNetwork testNetwork;
    
    public void Initialize()
    {
        testNetwork.ActionOnClientStart += Register;
        testNetwork.ActionOnClientStart += OnStartClient;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        namesList.OnChange += (arg1, arg2, arg3) =>
        {
            Debug.Log($"{arg1} {arg2} {arg3}");
        };
    }

    private void Register()
    {
        NetworkClient.RegisterHandler<ScoreMessage>(OnScore);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SendScore(250);
            
            namesList.Add(new Vector3(15,623,74));
        }
    }

    public void SendScore(int score)
    {
        ScoreMessage msg = new ScoreMessage { health = score };
        NetworkServer.SendToAll(msg);
    }

    public void OnScore(ScoreMessage msg)
    {
        Debug.Log("OnScoreMessage " + msg.health);
    }
}

public struct ScoreMessage : NetworkMessage
{
    public int health;
}