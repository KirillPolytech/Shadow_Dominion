using System;
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
            PositionMessage[] positionMessages)
        {
            _mirrorServer = mirrorServer;
            _playerFactory = playerFactory;
            _roomPlayerFactory = roomPlayerFactory;
            _positionMessages = positionMessages;
            
            Instance = this;
            
            _cachedOnDisconnect = arg => playerInstances.Remove(arg);

            _mirrorServer.ActionOnClientChangeScene += ActivatePlayerSpawn;
            _mirrorServer.ActionOnServerReady += ActivateRoomPlayerSpawn;
            _mirrorServer.ActionOnServerDisconnectWithArg += _cachedOnDisconnect;
        }

        ~MirrorPlayerSpawner()
        {
            _mirrorServer.ActionOnClientChangeScene -= ActivatePlayerSpawn;
            _mirrorServer.ActionOnServerDisconnectWithArg -= _cachedOnDisconnect;
        }
        
        [Client]
        private void ActivatePlayerSpawn()
        {
            PositionMessage message = new PositionMessage { pos = _positionMessages[LoadedPlayers].pos };
            //отправка сообщения на сервер с координатами спавна
            NetworkClient.Send(message);
        }

        [Client]
        private void ActivateRoomPlayerSpawn()
        {
            RoomPlayerSpawnMessage message = new RoomPlayerSpawnMessage();
            //отправка сообщения на сервер с координатами спавна
            NetworkClient.Send(message);

        }

        [Server]
        public void OnCreateCharacter(NetworkConnectionToClient conn, PositionMessage positionMessage)
        {
            //локально на сервере создаем gameObject
            Main.Player player = _playerFactory.Create();
            playerInstances.Add(conn, player);
            
            player.transform.SetPositionAndRotation(positionMessage.pos, Quaternion.identity);
            
            //присоеднияем gameObject к пулу сетевых объектов и отправляем информацию об этом остальным игрокам
            NetworkServer.AddPlayerForConnection(conn, player.gameObject);
            
            OnPlayerSpawned?.Invoke();
            
            LoadedPlayers++;
            
            Debug.Log($"[Server] OnCreateCharacter: {conn.address}");
        }
        
        [Server]
        public void OnCreateRoomPlayer(NetworkConnectionToClient conn, RoomPlayerSpawnMessage positionMessage)
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