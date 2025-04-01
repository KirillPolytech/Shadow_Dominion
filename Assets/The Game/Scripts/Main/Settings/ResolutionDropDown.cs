using TMPro;
using Zenject;

namespace Shadow_Dominion.Settings
{
    public class ResolutionDropDown : TMP_Dropdown
    {
        private ApplicationSettings _applicationSettings;
        
        [Inject]
        public void Construct(ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }
        
        private new void Start()
        {
            base.Start();

            onValueChanged.AddListener(ParseResolution);
        }

        private void ParseResolution(int option)
        {
            string resolution = options[option].text;
            
            string[] parts = resolution.Split('x');

            int width = int.Parse(parts[0]);
            int height = int.Parse(parts[1]);
            
            _applicationSettings.SetResolution(width, height);
        }
    }
}