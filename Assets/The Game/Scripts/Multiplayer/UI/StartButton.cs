using Mirror;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Shadow_Dominion
{
    public class StartButton : Button
    {
        private UnityAction _onServerChangeScene;

        [Inject]
        public void Construct(RoomSettings roomSettings)
        {
            _onServerChangeScene = () => NetworkManager.singleton.ServerChangeScene(roomSettings.mainLevel);
            
            MirrorServer.Instance.ActionOnHostStart += Subscribe;
            MirrorServer.Instance.ActionOnHostStop += Unsubscribe;
        }

        private void Subscribe() => onClick.AddListener(_onServerChangeScene.Invoke);
        private void Unsubscribe() => onClick.RemoveListener(_onServerChangeScene.Invoke);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            MirrorServer.Instance.ActionOnHostStart -= Subscribe;
            MirrorServer.Instance.ActionOnHostStop -= Unsubscribe;
        }
    }
}