using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Shadow_Dominion
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
            onClick.AddListener(() =>
            {
                if (!IPChecker.IsIPCorrect(_ipInputFieldProvider.TMPInputFields.text))
                {
                    Debug.LogWarning($"Ip incorrect: {_ipInputFieldProvider.TMPInputFields.text}");
                    return;
                }

                MirrorServer.Instance.networkAddress = _ipInputFieldProvider.TMPInputFields.text;
                MirrorServer.Instance.StartClient();
            });
        }
    }
}