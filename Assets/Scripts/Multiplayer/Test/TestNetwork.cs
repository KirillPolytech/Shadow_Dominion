using System;
using Mirror;
using UnityEngine;

public class TestNetwork : NetworkManager
{
    public Action ActionOnClientStart;

    public TestBehaviour testNetwork;

    public override void Start()
    {
        base.Start();
        
        testNetwork.Initialize();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartHost();
            Debug.Log("Host started");
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            StartClient();
            Debug.Log("Client started");
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        
        ActionOnClientStart?.Invoke();
    }
}
