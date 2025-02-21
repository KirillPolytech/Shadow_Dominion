using System;
using System.Collections;
using Mirror;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorLevel : MirrorSingleton<MirrorLevel>
    {
        public event Action OnAllPlayersLoaded;
        
        [SyncVar]
        private int _loadedPlayers;
        private Action _cachedStartCoroutine;
        private Coroutine _coroutine;

        private new void Awake()
        {
            base.Awake();
            
            _cachedStartCoroutine = () =>
            {
                if (_coroutine != null)
                    StopCoroutine(_coroutine);
                _coroutine = StartCoroutine(WaitForPlayer());
            };

            MirrorServer.Instance.ActionOnServerSceneChanged += _cachedStartCoroutine;
        }

        private void OnDestroy()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);
            
            MirrorServer.Instance.ActionOnServerSceneChanged -= _cachedStartCoroutine;
        }

        private IEnumerator WaitForPlayer()
        {
            while (MirrorPlayerSpawner.Instance.LoadedPlayers < 1)
            {
                yield return new WaitForSeconds(0.1f);
            }
            
            OnAllPlayersLoaded?.Invoke();
        }
    }
}