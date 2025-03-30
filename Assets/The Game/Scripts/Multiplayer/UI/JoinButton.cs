using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shadow_Dominion.UI
{
    public class JoinButton : Button
    {
        private IPInputFieldProvider _ipInputFieldProvider;
        private PORTInputFieldProvider _portInputFieldProvider;

        [Inject]
        public void Construct(IPInputFieldProvider ipInputFieldProvider,
            PORTInputFieldProvider portInputFieldProvider)
        {
            _ipInputFieldProvider = ipInputFieldProvider;
            _portInputFieldProvider = portInputFieldProvider;
        }

        protected override void Awake()
        {
            base.Awake();
            onClick.AddListener(StartClient);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            onClick.RemoveListener(StartClient);
        }

        private void StartClient()
        {
            if (!IPChecker.IsIPCorrect(_ipInputFieldProvider.TMPInputField.text))
            {
                Debug.LogWarning($"Ip incorrect: {_ipInputFieldProvider.TMPInputField.text}");
                return;
            }

            MirrorServer.Instance.networkAddress = _ipInputFieldProvider.TMPInputField.text;
            MirrorServer.Instance.StartClient();
        }
    }
}