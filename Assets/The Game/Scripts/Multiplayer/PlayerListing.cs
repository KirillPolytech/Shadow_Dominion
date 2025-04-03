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
                SpawnView(viewData);
            }
        }

        private void SpawnView(PlayerViewData playerViewData)
        {
            var keyValuePairs = _views.FirstOrDefault(x => x.Key == playerViewData.Nick);

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

            instance.SetName(playerViewData.Nick);
            instance.SetButtonState(playerViewData.IsReady);
            instance.SetNameColor(UserData.Instance.Nickname == playerViewData.Nick ? Color.green : Color.white);

            bool isAdded = _views.TryAdd(playerViewData.Nick, instance);

            if (!isAdded)
                Debug.LogWarning($"Cant add value by key: {playerViewData.Nick}");
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
    }
}