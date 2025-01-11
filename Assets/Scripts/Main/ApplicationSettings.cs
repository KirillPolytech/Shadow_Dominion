using UnityEngine;
using Zenject;

namespace Shadow_Dominion.Settings
{
    public class ApplicationSettings : IInitializable
    {
        public void Initialize()
        {
            Screen.SetResolution(1280, 720, FullScreenMode.Windowed);
        }
    }
}