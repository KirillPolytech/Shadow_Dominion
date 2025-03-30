using TMPro;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class MenuContext : MonoInstaller
    {
        [SerializeField]
        private MenuWindowsController menuWindowsController;

        [SerializeField]
        private RoomSettings roomSettings;

        [Space]
        [SerializeField]
        private TMP_InputField IP;

        [SerializeField]
        private TMP_InputField PORT;
        
        [SerializeField]
        private TMP_InputField nick;
        
        public override void InstallBindings()
        {
            IP.text = "127.0.0.1";
            PORT.text = "7777";

            Container.BindInstance(menuWindowsController).AsSingle();
            Container.BindInstance(roomSettings).AsSingle();

            Container.Bind<IPInputFieldProvider>().AsSingle().WithArguments(IP);
            Container.Bind<PORTInputFieldProvider>().AsSingle().WithArguments(PORT);
            Container.Bind<NickInputFieldProvider>().AsSingle().WithArguments(nick);
        }
    }
}