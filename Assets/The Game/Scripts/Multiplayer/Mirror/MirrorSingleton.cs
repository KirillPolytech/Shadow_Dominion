using Mirror;

namespace Shadow_Dominion
{
    public class MirrorSingleton<T> : NetworkBehaviour where T : NetworkBehaviour
    {
        public static T Instance { get; protected set; }

        protected void Awake()
        {
            if (Instance == null)
                Instance = this as T;
            else
                Destroy(gameObject);
        }
    }
}