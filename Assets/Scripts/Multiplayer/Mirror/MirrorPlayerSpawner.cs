using System;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Shadow_Dominion
{
    public class MirrorPlayerSpawner
    {
        private readonly MirrorServer _mirrorServer;
        private readonly PlayerFactory _playerFactory;
        private readonly PositionMessage[] _spawnPositions;
        private readonly Action<NetworkConnectionToClient> _cachedOnDisconnect;
        private readonly PositionMessage[] _positionMessages;

        public readonly Dictionary<NetworkConnectionToClient, Main.Player> playerInstances 
            = new Dictionary<NetworkConnectionToClient, Main.Player>();

        public static MirrorPlayerSpawner Instance { get; private set; }
        
        public event Action OnPlayerSpawned;

        public int LoadedPlayers { get; private set; }

        [Inject]
        public MirrorPlayerSpawner(
            MirrorServer mirrorServer, 
            PlayerFactory playerFactory,
            PositionMessage[] positionMessages)
        {
            _mirrorServer = mirrorServer;
            _playerFactory = playerFactory;
            _positionMessages = positionMessages;
            
            Instance = this;
            
            _cachedOnDisconnect = arg => playerInstances.Remove(arg);

            _mirrorServer.ActionOnHostStart += RegisterHandler;
            _mirrorServer.ActionOnClientChangeScene += ActivatePlayerSpawn;
            _mirrorServer.ActionOnServerDisconnectWithArg += _cachedOnDisconnect;
        }

        ~MirrorPlayerSpawner()
        {
            _mirrorServer.ActionOnHostStart -= RegisterHandler;
            _mirrorServer.ActionOnClientChangeScene -= ActivatePlayerSpawn;
            _mirrorServer.ActionOnServerDisconnectWithArg -= _cachedOnDisconnect;
        }

        private void RegisterHandler()
        {
            //указываем, какой struct должен прийти на сервер, чтобы выполнился свапн
            NetworkServer.RegisterHandler<PositionMessage>(OnCreateCharacter);
        }
        
        private void ActivatePlayerSpawn()
        {
            float value = Random.Range(-5, 5);
            Vector3 newpos = new Vector3(value, 2, value);
            //создаем struct определенного типа, чтобы сервер понял к чему эти данные относятся
            
            PositionMessage message = new PositionMessage { pos = _positionMessages[LoadedPlayers].pos };
            //отправка сообщения на сервер с координатами спавна
            NetworkClient.Send(message);
        }

        private void OnCreateCharacter(NetworkConnectionToClient conn, PositionMessage positionMessage)
        {
            //локально на сервере создаем gameObject
            Main.Player player = _playerFactory.Create();
            playerInstances.Add(conn, player);
            
            player.transform.SetPositionAndRotation(positionMessage.pos, Quaternion.identity);
            
            //присоеднияем gameObject к пулу сетевых объектов и отправляем информацию об этом остальным игрокам
            NetworkServer.ReplacePlayerForConnection(conn, player.gameObject, ReplacePlayerOptions.Destroy);
            
            OnPlayerSpawned?.Invoke();
            
            LoadedPlayers++;
            
            Debug.Log($"OnCreateCharacter: {conn.address}");
        }
    }
}