using Multiplayer.Structs;
using Shadow_Dominion.StateMachine;
using UnityEngine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class InActiveState : IState
    {
        private readonly Main.Player _player;
        private readonly PositionMessage[] _positionMessage;
        
        public InActiveState(Main.Player player, PositionMessage[] positionMessage)
        {
            _player = player;
            _positionMessage = positionMessage;
        }
        
        public override void Enter()
        {
            Vector3 dir = new Vector3(25, 0, 25) - _player.PlayersTrasform.position;
            dir.y = 0;
            Quaternion lookRot = Quaternion.LookRotation(dir);
            _player.SetPositionAndRotation(_positionMessage[Random.Range(0,_positionMessage.Length)].Position, lookRot);
        }

        public override void Exit()
        {
            
        }
    }
}