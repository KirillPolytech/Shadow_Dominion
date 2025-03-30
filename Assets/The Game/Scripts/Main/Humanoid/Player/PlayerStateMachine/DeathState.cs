using Shadow_Dominion.AnimStateMachine;

namespace Shadow_Dominion.Player.StateMachine
{
    public class DeathState : PlayerState
    {
        private readonly BoneController[] _boneControllers;
        private readonly CameraLook _cameraLook;
        
        public DeathState(
            PlayerAnimation playerAnimation,
            BoneController[] boneControllers,
            CameraLook cameraLook) : base(playerAnimation)
        {
            _boneControllers = boneControllers;
            _cameraLook = cameraLook;
        }
        
        public override void Enter()
        {
            _playerAnimation.AnimationStateMachine.SetState<AnimationLay>();
            
            //_rigBuilder.enabled = false;
            _cameraLook.CanZooming = false;
            
            for (int i = 0; i < _boneControllers.Length; i++)
            {
                _boneControllers[i].IsPositionApplying(false);
                _boneControllers[i].IsRotationApplying(false);
            }
        }

        public override void Exit()
        {
            
        }

        public override bool CanExit() => true;
    }
}