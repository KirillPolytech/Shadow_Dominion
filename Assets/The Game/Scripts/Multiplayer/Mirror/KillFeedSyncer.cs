using Mirror;

namespace Shadow_Dominion
{
    public class KillFeedSyncer : MirrorSingleton<KillFeedSyncer>
    {
        private new void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(gameObject);
        }
        
        [Command(requiresAuthority = false)]
        public void AddFeed(string killerName, string victimName)
        {
            UpdateFeed(killerName, victimName);
        }

        [ClientRpc]
        private void UpdateFeed(string killerName, string victimName)
        {
            KillFeed.Instance.AddFeed(killerName, victimName);
        }
    }
}