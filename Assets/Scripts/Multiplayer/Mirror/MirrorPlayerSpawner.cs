using Mirror;
using UnityEngine;

namespace Shadow_Dominion
{
    public class MirrorPlayerSpawner
    {
        private readonly MirrorServer _mirrorServer;
        private readonly PlayerPool _playerPool;
        private readonly PositionMessage[] _spawnPositions;

        public MirrorPlayerSpawner(MirrorServer mirrorServer, PlayerPool playerPool, PositionMessage[] spawnPositions)
        {
            _mirrorServer = mirrorServer;
            _playerPool = playerPool;

            _mirrorServer.ActionOnHostStart += RegisterHandler;
            _mirrorServer.ActionOnClientChangeScene += ActivatePlayerSpawn;
        }

        ~MirrorPlayerSpawner()
        {
            _mirrorServer.ActionOnHostStart -= RegisterHandler;
            _mirrorServer.ActionOnClientChangeScene -= ActivatePlayerSpawn;
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
            PositionMessage message = new PositionMessage { pos = newpos };
            //отправка сообщения на сервер с координатами спавна
            NetworkClient.Send(message);
        }

        private void OnCreateCharacter(NetworkConnectionToClient conn, PositionMessage positionMessage)
        {
            //локально на сервере создаем gameObject
            GameObject go = _playerPool.Pull().gameObject;
            go.transform.SetPositionAndRotation(positionMessage.pos, Quaternion.identity);
            //присоеднияем gameObject к пулу сетевых объектов и отправляем информацию об этом остальным игрокам
            NetworkServer.AddPlayerForConnection(conn, go);
            Debug.Log($"OnCreateCharacter: {conn.address}");
        }
    }
}