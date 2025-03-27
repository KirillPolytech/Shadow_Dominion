using UnityEngine;
using UnityEngine.UI;
using WindowsSystem;
using Zenject;

namespace Shadow_Dominion.UI
{
    public class ConfirmNickButton : Button
    {
        private InputFieldsProvider _inputFieldProvider;
        private WindowsController _windowsController;

        [Inject]
        public void Construct(
            NickInputFieldProvider nickInputFieldProvider,
            MenuWindowsController windowsController)
        {
            _inputFieldProvider = nickInputFieldProvider;
            _windowsController = windowsController;
        }

        protected override void Awake()
        {
            base.Awake();

            onClick?.AddListener(SetNick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            onClick?.RemoveListener(SetNick);
        }

        private void SetNick()
        {
            if (_inputFieldProvider.TMPInputField.text == string.Empty)
            {
                Debug.LogWarning("Nick is null");
                return;
            }

            UserData.Instance.Nickname = _inputFieldProvider.TMPInputField.text;

            _windowsController.OpenWindow<JoinWindow>();
        }
    }
}