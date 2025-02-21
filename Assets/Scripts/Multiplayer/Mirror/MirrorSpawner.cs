using Mirror;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class MirrorSpawner : NetworkBehaviour
    {
        [SerializeField]
        private MirrorLevel mirrorLevelPrefab;

        [SerializeField]
        private MirrorLobby mirrorLobbyPrefab;

        private MirrorServer _mirrorServer;
        private IInstantiator _instantiator;

        [Inject]
        public void Construct(MirrorServer mirrorServer, IInstantiator instantiator)
        {
            _mirrorServer = mirrorServer;
            _instantiator = instantiator;
        }

        private void Start()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);

            _mirrorServer.ActionOnServerConnect += Dispawn;
            _mirrorServer.ActionOnServerConnect += Spawn;
        }

        private void OnDestroy()
        {
            
        }

        private void Dispawn()
        {
            if (MirrorLevel.Instance)
                NetworkServer.UnSpawn(MirrorLevel.Instance.gameObject);
            if (MirrorLobby.Instance)
                NetworkServer.UnSpawn(MirrorLobby.Instance.gameObject);
        }

        private void Spawn()
        {
            if (!MirrorLevel.Instance)
                _instantiator.InstantiatePrefab(mirrorLevelPrefab);
            NetworkServer.Spawn(MirrorLevel.Instance.gameObject);

            if (!MirrorLobby.Instance)
                _instantiator.InstantiatePrefab(mirrorLobbyPrefab);
            NetworkServer.Spawn(MirrorLobby.Instance.gameObject);
        }
    }
}