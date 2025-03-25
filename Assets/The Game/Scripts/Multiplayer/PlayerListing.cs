using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class PlayerListing : Singleton<PlayerListing>
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

        public void SpawnView(string address)
        {
            var keyValuePairs = _views.FirstOrDefault(x => x.Key == address);

            if (keyValuePairs.Equals(null))
                return;

            RoomPlayerView instance = _instantiator.InstantiatePrefab(roomViewPrefab).GetComponent<RoomPlayerView>();
            instance.gameObject.transform.SetParent(content);
            ((RectTransform) instance.transform).localScale = Vector3.one;
            ((RectTransform) instance.transform).anchoredPosition3D = Vector3.zero;
            ((RectTransform) instance.transform).position = Vector3.zero;
            ((RectTransform) instance.transform).localPosition = Vector3.zero;

            instance.SetName(address);

            bool isAdded = _views.TryAdd(address, instance);

            if (!isAdded)
                Debug.LogWarning($"Cant add value by key: {address}");
        }
        
        public void SpawnView(PlayerViewData[] addresses)
        {
            for (int i = 0; i < _views.Count; i++)
            {
                Destroy(_views.ElementAt(i).Value.gameObject);
            }
            
            _views.Clear();
            
            foreach (var address in addresses)
            {
                SpawnView(address.Address);
            }
        }

        public void DispawView(string address)
        {
            RoomPlayerView instance = _views[address];
            _views.Remove(address);
            Destroy(instance.gameObject);
        }

        public void IsReady(string address, bool state)
        {
            _views[address].SetButtonState(state);
        }
    }
}