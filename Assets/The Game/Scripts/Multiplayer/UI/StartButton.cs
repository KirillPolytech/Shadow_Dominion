using System.Linq;
using Mirror;
using The_Game.Scripts.Main;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Shadow_Dominion
{
    public class StartButton : Button
    {
        private UnityAction _onServerChangeScene;
        private bool _isInitialized;

        [Inject]
        public void Construct(RoomSettings roomSettings)
        {
            if (SceneManager.GetActiveScene().name != SceneNamesStorage.OnlineMenuScene)
                return;

            _onServerChangeScene = () =>
            {
                bool isAllReady = MirrorPlayersSyncer.Instance.Players.All(playerViewData => playerViewData.IsReady);

                if (!isAllReady)
                    return;

                NetworkManager.singleton.ServerChangeScene(roomSettings.mainLevel);
            };

            MirrorServer.Instance.ActionOnHostStart += Enable;
            MirrorServer.Instance.ActionOnHostStart += Subscribe;
            MirrorServer.Instance.ActionOnHostStop += Unsubscribe;

            gameObject.SetActive(false);

            _isInitialized = true;
        }

        private void Subscribe() => onClick.AddListener(_onServerChangeScene.Invoke);
        private void Unsubscribe() => onClick.RemoveListener(_onServerChangeScene.Invoke);

        private void Enable() => gameObject.SetActive(true);

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (!_isInitialized)
                return;

            MirrorServer.Instance.ActionOnHostStart -= Enable;
            MirrorServer.Instance.ActionOnHostStart -= Subscribe;
            MirrorServer.Instance.ActionOnHostStop -= Unsubscribe;
        }
    }
}