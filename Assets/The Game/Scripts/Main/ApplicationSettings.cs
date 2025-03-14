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
                (int)_applicationSettingsSo.Resolution.x, (int)_applicationSettingsSo.Resolution.y, _applicationSettingsSo.ScreenMode);
            
            Application.targetFrameRate = _applicationSettingsSo.TargetFPS;
        }
    }
}