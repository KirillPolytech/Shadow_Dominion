using System;
using Mirror;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

namespace Shadow_Dominion
{
    public class StartButton : Button
    {
        private UnityAction _onServerChangeScene;
        private Action _setState;
        private bool _isInitialized;

        [Inject]
        public void Construct(RoomSettings roomSettings)
        {
            _onServerChangeScene = () => NetworkManager.singleton.ServerChangeScene(roomSettings.mainLevel);
            
            _setState = () => gameObject.SetActive(true);
            
            MirrorServer.Instance.ActionOnHostStart += _setState.Invoke;
            MirrorServer.Instance.ActionOnHostStart += Subscribe;
            MirrorServer.Instance.ActionOnHostStop += Unsubscribe;

            gameObject.SetActive(false);
            
            _isInitialized = true;
        }

        private void Subscribe() => onClick.AddListener(_onServerChangeScene.Invoke);
        private void Unsubscribe() => onClick.RemoveListener(_onServerChangeScene.Invoke);

        protected override void OnDestroy()
        {
            base.OnDestroy();
            
            if (!_isInitialized)
                return;
            
            MirrorServer.Instance.ActionOnHostStart -= _setState.Invoke;
            MirrorServer.Instance.ActionOnHostStart -= Subscribe;
            MirrorServer.Instance.ActionOnHostStop -= Unsubscribe;
        }
    }
}