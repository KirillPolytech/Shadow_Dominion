using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class PlayerListing : MonoSingleton<PlayerListing>
    {
        private readonly Dictionary<string, RoomPlayerView> _views = new();

        [SerializeField]
        private RectTransform content;

        [SerializeField]
        private RoomPlayerView roomViewPrefab;

        private IInstantiator _instantiator;

        [Inject]
        public void Construct(IInstantiator instantiator)
        {
            _instantiator = instantiator;
        }
        
        public void SpawnView(PlayerViewData[] playerViewData)
        {
            DispawnView();
            
            foreach (var viewData in playerViewData)
            {
                SpawnView(viewData.Nick);
            }
        }

        private void SpawnView(string nick)
        {
            var keyValuePairs = _views.FirstOrDefault(x => x.Key == nick);

            if (keyValuePairs.Equals(null))
                return;

            RoomPlayerView instance = _instantiator.InstantiatePrefab(roomViewPrefab).GetComponent<RoomPlayerView>();
            instance.gameObject.transform.SetParent(content);

            RectTransform rectTransform = (RectTransform) instance.transform;
            rectTransform.localScale = Vector3.one;
            rectTransform.anchoredPosition3D = Vector3.zero;
            rectTransform.position = Vector3.zero;
            rectTransform.localRotation = Quaternion.identity;
            rectTransform.localPosition = Vector3.zero;

            instance.SetName(nick);

            bool isAdded = _views.TryAdd(nick, instance);

            if (!isAdded)
                Debug.LogWarning($"Cant add value by key: {nick}");
        }

        private void DispawnView()
        {
            for (int i = 0; i < _views.Count; i++)
            {
                if (_views.ElementAt(i).Value)
                    Destroy(_views.ElementAt(i).Value.gameObject);
            }
            
            _views.Clear();
        }

        public void IsReady(string nick, bool state)
        {   
            _views[nick].SetButtonState(state);
            
            Debug.Log($"{nick} IsReady: {state}");
        }
    }
}