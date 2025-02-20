using Mirror;

namespace Shadow_Dominion
{
    public class NetworkBehavioursProvider
    {
        public NetworkBehaviour[] NetworkBehaviours;

        public NetworkBehavioursProvider(NetworkBehaviour[] networkBehaviours)
        {
            NetworkBehaviours = networkBehaviours;
        }
    }
}