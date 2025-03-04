using System;
using Mirror;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class MirrorSpawner : MonoBehaviour
    {
        [SerializeField]
        private MirrorLevel mirrorLevelPrefab;

        [SerializeField]
        private MirrorLobby mirrorLobbyPrefab;

        [SerializeField]
        private MirrorPlayerStateSyncer mirrorPlayerStateSyncer;

        [SerializeField]
        private MirrorPlayersSyncer mirrorPlayersSyncer;

        private MirrorServer _mirrorServer;
        private IInstantiator _instantiator;
        private MirrorRegister _mirrorRegister;

        [Inject]
        public void Construct(MirrorServer mirrorServer, IInstantiator instantiator)
        {
            _mirrorServer = mirrorServer;
            _instantiator = instantiator;
            _mirrorRegister = new MirrorRegister();
        }

        private void Start()
        {
            transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                Debug.Log("Key pressed");
                Spawn();
            }
        }

        public void Dispawn()
        {
            _mirrorRegister.UnRegister();

            UnspawnObject(MirrorLevel.Instance);
            UnspawnObject(MirrorLobby.Instance);
            UnspawnObject(MirrorPlayerStateSyncer.Instance);
            UnspawnObject(MirrorPlayersSyncer.Instance);
        }

        public void Spawn()
        {
            SpawnObject(MirrorLevel.Instance, mirrorLevelPrefab);
            SpawnObject(MirrorLobby.Instance, mirrorLobbyPrefab);
            SpawnObject(MirrorPlayerStateSyncer.Instance, mirrorPlayerStateSyncer);
            SpawnObject(MirrorPlayersSyncer.Instance, mirrorPlayersSyncer);

            _mirrorRegister.Register();
        }

        private void SpawnObject<T>(T instance, T prefab) where T : MonoBehaviour
        {
            if (instance)
                return;

            instance = _instantiator.InstantiatePrefabForComponent<T>(prefab);
            NetworkServer.Spawn(instance.gameObject);
            Debug.Log($"[MirrorSpawner] Spawned: {typeof(T).Name}");
        }

        private void UnspawnObject<T>(T instance) where T : MonoBehaviour
        {
            if (instance && instance.gameObject && NetworkServer.active)
            {
                NetworkServer.UnSpawn(instance.gameObject);
                Destroy(instance.gameObject);
                Debug.Log($"[MirrorSpawner] Unspawned: {typeof(T).Name}");
            }
        }
    }
}