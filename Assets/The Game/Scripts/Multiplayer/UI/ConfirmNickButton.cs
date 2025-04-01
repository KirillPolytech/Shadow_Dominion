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
        private TextSO _textSo;
        private bool _isInitialized;

        [Inject]
        public void Construct(
            NickInputFieldProvider nickInputFieldProvider,
            MenuWindowsController windowsController,
            TextSO textSo)
        {
            _inputFieldProvider = nickInputFieldProvider;
            _windowsController = windowsController;
            _textSo = textSo;
            
            _inputFieldProvider.TMPInputField.onValueChanged.AddListener(OnValueChanged);

            _isInitialized = true;
        }

        protected override void Awake()
        {
            base.Awake();

            onClick?.AddListener(SetNick);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (!_isInitialized)
                return;

            onClick?.RemoveListener(SetNick);
            _inputFieldProvider.TMPInputField.onValueChanged.RemoveListener(OnValueChanged);
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

        private void OnValueChanged(string text)
        {
            string result = text.Length > _textSo.NickLength ? text[.._textSo.NickLength] : text;
            _inputFieldProvider.TMPInputField.text = result;
        }
    }
}