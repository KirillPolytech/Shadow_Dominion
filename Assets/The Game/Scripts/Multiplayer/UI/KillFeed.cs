using System.Collections.Generic;
using UnityEngine;

namespace Shadow_Dominion
{
    public class KillFeed : MonoSingleton<KillFeed>
    {
        private readonly List<KillFeedView> _instances = new();

        [SerializeField] private Transform content;
        [SerializeField] private KillFeedView viewPrefab;

        public void AddFeed(string killer, string victim)
        {
            if (_instances.Count > 10)
                Debug.Log("There is more than 10 Kill Feeds");
            
            KillFeedView killFeedView = Instantiate(viewPrefab, content);
            killFeedView.SetKiller(killer == string.Empty
                ? $"<color=red>{victim}</color>"
                : $"<color=green>{killer}</color>");

            killFeedView.SetVictim($"<color=red>{victim}</color>");

            _instances.Add(killFeedView);
        }

        public void Reset()
        {
            foreach (var killFeedView in _instances)
            {
                Destroy(killFeedView.gameObject);
            }

            _instances.Clear();
        }
    }
}