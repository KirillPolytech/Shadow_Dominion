using System.Linq;
using Shadow_Dominion.StateMachine;
using WindowsSystem;
using Zenject;

public class LevelStateMachine : IStateMachine
{
    [Inject]
    public LevelStateMachine(CursorService cursorService, WindowsController windowsController)
    {
        _states.Add(new GameplayState(cursorService));
        _states.Add(new PauseState(windowsController));
        
        SetState<GameplayState>();
    }
    
    public sealed override void SetState<T>()
    {
        IState state = _states.First(x => x.GetType() == typeof(T));
        
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }
}
