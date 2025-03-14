using UnityEngine;

namespace Shadow_Dominion.Settings
{
    [CreateAssetMenu(fileName = "ApplicationSettings", menuName = PathStorage.ScriptableObjectMenu + "/ApplicationSettings")]
    public class ApplicationSettingsSO : ScriptableObject
    {
        public int TargetFPS = 60;
        public Vector2 Resolution = new Vector2(1280, 720);
        public FullScreenMode ScreenMode = FullScreenMode.Windowed;
    }
}