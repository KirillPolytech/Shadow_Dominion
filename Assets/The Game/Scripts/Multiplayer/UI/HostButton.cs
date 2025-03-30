using Mirror;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shadow_Dominion
{
    public class HostButton : Button
    {
        private IPInputFieldProvider _ipInputFieldProvider;
        private PORTInputFieldProvider _portInputFieldProvider;

        [Inject]
        public void Construct(
            IPInputFieldProvider ipInputFieldProvider,
            PORTInputFieldProvider portInputFieldProvider)
        {
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
            if (!IPChecker.IsIPCorrect(_ipInputFieldProvider.TMPInputField.text))
            {
                Debug.LogWarning($"Ip incorrect: {_ipInputFieldProvider.TMPInputField.text}");
                return;
            }

            MirrorServer.Instance.networkAddress = _ipInputFieldProvider.TMPInputField.text;
            
            if (!NetworkServer.active)
                MirrorServer.Instance.StartHost();
            else
                MirrorServer.Instance.StartClient();
        }
    }
}