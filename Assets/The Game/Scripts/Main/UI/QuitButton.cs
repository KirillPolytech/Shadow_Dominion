using Shadow_Dominion.Settings;
using UnityEngine.UI;
using Zenject;

namespace Shadow_Dominion
{
    public class QuitButton : Button
    {
        private ApplicationSettings _applicationSettings;
        private bool _isInitialized;
        
        [Inject]
        public void Construct(ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
            
            onClick.AddListener(_applicationSettings.Quit);

            _isInitialized = true;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (!_isInitialized)
                return;
            
            onClick.RemoveListener(_applicationSettings.Quit);
        }
    }
}