using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HostButton : Button
{
    private MirrorServer _mirrorServer;
    private IPInputFieldProvider _ipInputFieldProvider;
    private PORTInputFieldProvider _portInputFieldProvider;

    [Inject]
    public void Construct(
        MirrorServer mirrorServer, 
        IPInputFieldProvider ipInputFieldProvider, 
        PORTInputFieldProvider portInputFieldProvider)
    {
        _mirrorServer = mirrorServer;

        _ipInputFieldProvider = ipInputFieldProvider;
        _portInputFieldProvider = portInputFieldProvider;
    }

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(StartHost);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();        
        onClick.RemoveListener(StartHost);
    }

    private void StartHost()
    {
        if (!IPChecker.IsIPCorrect(_ipInputFieldProvider.TMPInputFields.text))
        {
            Debug.LogWarning($"Ip incorrect: {_ipInputFieldProvider.TMPInputFields.text}");
            return;
        }
            
        _mirrorServer.networkAddress = _ipInputFieldProvider.TMPInputFields.text;
        _mirrorServer.StartHost();
    }
}