using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Shadow_Dominion
{
    public class KillFeed : MonoSingleton<KillFeed>
    {
        private readonly List<TextMeshProUGUI> _instances = new();
        
        [SerializeField] private Transform content;
        [SerializeField] private TextMeshProUGUI viewPrefab;

        public void AddFeed(string killer, string victim)
        {
            TextMeshProUGUI text = Instantiate(viewPrefab, content);
            text.text = $"<color=green>{killer}</color> killed <color=red>{victim}</color>";
            
            _instances.Add(text);
        }

        public void Reset()
        {
            
        }
    }
}