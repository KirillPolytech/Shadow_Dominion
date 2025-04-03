using System;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion.Settings
{
    public class ApplicationSettings : IInitializable
    {
        private readonly ApplicationSettingsSO _applicationSettingsSo;

        public ApplicationSettings(ApplicationSettingsSO applicationSettingsSo)
        {
            _applicationSettingsSo = applicationSettingsSo;
        }

        public void Initialize()
        {
            Screen.SetResolution(
                (int) _applicationSettingsSo.Resolution.x, (int) _applicationSettingsSo.Resolution.y,
                _applicationSettingsSo.ScreenMode);

            Application.targetFrameRate = _applicationSettingsSo.TargetFPS;
        }

        public void SetResolution(int x, int y)
        {
            if (x <= 0 || y <= 0)
                throw new ArgumentOutOfRangeException();
            
            Screen.SetResolution(x, y, Screen.fullScreenMode);
        }

        public void SetVSYNC(int value)
        {
            if (value is < 0 or > 2)
                throw new ArgumentOutOfRangeException();

            QualitySettings.vSyncCount = value;
        }

        public void SetScreenMode(FullScreenMode fullScreenMode)
        {
            Screen.SetResolution(Screen.width, Screen.height, fullScreenMode);
        }

        public void SetQuality(int value)
        {
            string[] names = QualitySettings.names;

            if (value < 0 || value >= names.Length)
                throw new ArgumentOutOfRangeException();

            QualitySettings.SetQualityLevel(value);
        }

        public void SetMSAA(int value)
        {
            if (value < 0 || value > 8 && value % 2 != 0)
                throw new ArgumentOutOfRangeException();

            QualitySettings.antiAliasing = value;
        }

        public void Quit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
        }
    }
}