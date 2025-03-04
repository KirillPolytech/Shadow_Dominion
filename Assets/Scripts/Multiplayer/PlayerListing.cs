using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shadow_Dominion
{
    public class PlayerListing : Singleton<PlayerListing>
    {
        private readonly Dictionary<string, Transform> _transforms = new Dictionary<string, Transform>();
        
        [SerializeField]
        private RectTransform content;

        [SerializeField]
        private RectTransform roomViewPrefab;

        public void SpawnView(string address)
        {
            _transforms.TryGetValue(address, out var value);
            
            if (value)
                return;
            
            Transform instance = Instantiate(roomViewPrefab.transform, content);

            instance.GetComponent<TextMeshProUGUI>().text = address;
            
            bool isAdded = _transforms.TryAdd(address, instance);
            
            if (!isAdded)
                Debug.LogWarning($"Cant add value by key: {address}");
        }

        public void Dispawn(string address)
        {
           Transform instance = _transforms[address];
           _transforms.Remove(address);
           Destroy(instance.gameObject);
        }
    }
}