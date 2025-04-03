using Shadow_Dominion.Settings;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class QualitySlider : Slider
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

    private void OnValueChanged(float val)
    {
        int intVal = (int)Mathf.Clamp(val, 0, QualitySettings.names.Length);
        
        _applicationSettings.SetQuality(intVal);
    }
}
