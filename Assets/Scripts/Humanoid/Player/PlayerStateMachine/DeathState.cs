namespace Shadow_Dominion.Player.StateMachine
{
    public class DeathState : PlayerState
    {
        private readonly BoneController[] _boneControllers;
        
        public DeathState(
            PlayerAnimation playerAnimation,
            BoneController[] boneControllers) : base(playerAnimation)
        {
            _boneControllers = boneControllers;
        }
        
        public override void Enter()
        {
            for (int i = 0; i < _boneControllers.Length; i++)
            {
                _boneControllers[i].IsPositionApplying(false);
                _boneControllers[i].IsRotationApplying(false);
            }
        }

        public override void Exit()
        {
            
        }
    }
}