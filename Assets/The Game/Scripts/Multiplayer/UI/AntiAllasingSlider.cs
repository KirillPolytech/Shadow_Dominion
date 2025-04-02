using Shadow_Dominion.Settings;
using UnityEngine.UI;
using Zenject;

public class AntiAllasingSlider : Slider
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

    private void OnValueChanged(float v)
    {
        _applicationSettings.SetMSAA((int)(v % 2));
    }
}