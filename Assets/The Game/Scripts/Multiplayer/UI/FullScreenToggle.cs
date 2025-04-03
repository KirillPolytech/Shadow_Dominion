using Shadow_Dominion.Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class FullScreenToggle : Toggle
{
    private ApplicationSettings _applicationSettings;
    
    [Inject]
    public void Construct(ApplicationSettings applicationSettings)
    {
        _applicationSettings = applicationSettings;

        
        onValueChanged.AddListener(OnValueChanged);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        onValueChanged.RemoveListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
        FullScreenMode fullScreenMode = value ? FullScreenMode.FullScreenWindow : FullScreenMode.MaximizedWindow;
        _applicationSettings.SetScreenMode(fullScreenMode);
    }
}