using System;
using System.Collections;
using System.Collections.Generic;
using Mirror;
using Multiplayer.Structs;
using UnityEngine;
using Zenject;

namespace Shadow_Dominion
{
    public class MirrorPlayerSpawner
    {
        private readonly MirrorServer _mirrorServer;
        private readonly PlayerFactory _playerFactory;
        private readonly RoomPlayerFactory _roomPlayerFactory;
        private readonly PositionMessage[] _spawnPositions;
        private readonly Action<NetworkConnectionToClient> _cachedOnDisconnect;
        private readonly PositionMessage[] _positionMessages;
        private readonly CoroutineExecuter _coroutineExecuter;

        public readonly Dictionary<NetworkConnectionToClient, Main.Player> playerInstances 
            = new Dictionary<NetworkConnectionToClient, Main.Player>();
        
        public readonly Dictionary<NetworkConnectionToClient, NetworkRoomPlayer> roomPlayerInstances 
            = new Dictionary<NetworkConnectionToClient, NetworkRoomPlayer>();

        public static MirrorPlayerSpawner Instance { get; private set; }
        
        public event Action OnPlayerSpawned;

        public int LoadedPlayers { get; private set; }

        [Inject]
        public MirrorPlayerSpawner(
            MirrorServer mirrorServer, 
            PlayerFactory playerFactory,
            RoomPlayerFactory roomPlayerFactory,
            PositionMessage[] positionMessages,
            CoroutineExecuter coroutineExecuter)
        {
            _mirrorServer = mirrorServer;
            _playerFactory = playerFactory;
            _roomPlayerFactory = roomPlayerFactory;
            _positionMessages = positionMessages;
            _coroutineExecuter = coroutineExecuter;
            
            Instance = this;
            
            NetworkServer.RegisterHandler<RoomPlayerSpawnMessage>(OnCreateRoomPlayer);
            
            _cachedOnDisconnect = arg => playerInstances.Remove(arg);

            _mirrorServer.ActionOnClientChangedSceneWithArg += ActivatePlayerSpawn;
            _mirrorServer.ActionOnServerConnect += ActivateRoomPlayerSpawn;
            _mirrorServer.ActionOnServerDisconnectWithArg += _cachedOnDisconnect;
        }

        ~MirrorPlayerSpawner()
        {
            NetworkServer.UnregisterHandler<RoomPlayerSpawnMessage>();
            
            _mirrorServer.ActionOnClientChangedSceneWithArg -= ActivatePlayerSpawn;
            _mirrorServer.ActionOnServerConnect -= ActivateRoomPlayerSpawn;
            _mirrorServer.ActionOnServerDisconnectWithArg -= _cachedOnDisconnect;
        }
        
        [Client]
        private void ActivatePlayerSpawn(NetworkConnection conn)
        {
            PositionMessage message = new PositionMessage { pos = _positionMessages[LoadedPlayers].pos };
            //отправка сообщения на сервер с координатами спавна
            NetworkClient.Send(message);
        }

        [Client]
        private void ActivateRoomPlayerSpawn()
        {
            NetworkClient.Ready();
            
            RoomPlayerSpawnMessage message = new RoomPlayerSpawnMessage();
            NetworkClient.Send(message);
        }

        [Server]
        public void OnCreateCharacter(NetworkConnectionToClient conn, PositionMessage positionMessage)
        {
            return;
            
            //локально на сервере создаем gameObject
            Main.Player player = _playerFactory.Create();
            playerInstances.Add(conn, player);
            
            player.transform.SetPositionAndRotation(positionMessage.pos, Quaternion.identity);

            _coroutineExecuter.Execute(WaitForReady(conn, player));
        }
        
        private IEnumerator WaitForReady(NetworkConnectionToClient conn, Main.Player player)
        {
            while (!conn.isReady)
            {
                yield return new WaitForFixedUpdate();
            }
            
            //присоеднияем gameObject к пулу сетевых объектов и отправляем информацию об этом остальным игрокам
            NetworkServer.ReplacePlayerForConnection(conn, player.gameObject, ReplacePlayerOptions.Destroy);
            
            OnPlayerSpawned?.Invoke();
            
            LoadedPlayers++;
            
            Debug.Log($"[Server] OnCreateCharacter: {conn.address}");
        }
        
        [Server]
        private void OnCreateRoomPlayer(NetworkConnectionToClient conn, RoomPlayerSpawnMessage positionMessage)
        {
            NetworkRoomPlayer player = _roomPlayerFactory.Create();
            roomPlayerInstances.Add(conn, player);
            
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            
            OnPlayerSpawned?.Invoke();
            
            LoadedPlayers++;
            
            Debug.Log($"[Server] OnCreateRoomPlayer: {conn.address}");
        }
    }
}