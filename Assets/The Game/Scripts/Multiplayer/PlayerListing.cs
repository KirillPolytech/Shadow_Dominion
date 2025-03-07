using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Shadow_Dominion
{
    public class PlayerListing : Singleton<PlayerListing>
    {
        private readonly Dictionary<string, RoomPlayerView> _views = new Dictionary<string, RoomPlayerView>();

        [SerializeField]
        private RectTransform content;

        [SerializeField]
        private RoomPlayerView roomViewPrefab;

        public void SpawnView(string address)
        {
            var keyValuePairs = _views.FirstOrDefault(x => x.Key == address);

            if (keyValuePairs.Equals(null))
                return;

            RoomPlayerView instance = Instantiate(roomViewPrefab, content);

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
            _views[address].SetCheckMarkState(state);
        }
    }
}