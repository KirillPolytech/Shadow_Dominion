using System.Linq;
using HellBeavers.Level.StateMachine;
using Zenject;

public class LevelStateMachine : IStateMachine
{
    [Inject]
    public LevelStateMachine(CursorService cursorService)
    {
        _states.Add(new GameplayState(cursorService));
        _states.Add(new PauseState());
        
        SetState<GameplayState>();
    }
    
    public override void SetState<T>()
    {
        CurrentState?.Exit();
        IState state = _states.First(x => x.GetType() == typeof(T));
        state.Enter();
        CurrentState = state;
    }
}
