using HellBeavers.Level.StateMachine;
using Zenject;

public class LevelStateMachine : IStateMachine
{
    [Inject]
    public LevelStateMachine(CursorService cursorService)
    {
        _states.Add(new GameplayState(cursorService));
        _states.Add(new PauseState());
    }
    
    public override void SetState<T>()
    {
        
    }
}
