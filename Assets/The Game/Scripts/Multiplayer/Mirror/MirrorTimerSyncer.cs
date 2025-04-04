using Mirror;

namespace Shadow_Dominion
{
    public class MirrorTimerSyncer : MirrorSingleton<MirrorTimerSyncer>
    {
        private new void Awake()
        {
            base.Awake();
            
            DontDestroyOnLoad(gameObject);
        }
        
        [SyncVar] public float LevelInitializeTimer;
    }
}