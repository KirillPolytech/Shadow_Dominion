using System.Linq;
using HellBeavers.Level.StateMachine;
using UnityEngine;

public class AnimationStateMachine : IStateMachine
{
    public AnimationStateMachine(Animator animator)
    {
        _states.Add(new IdleState(animator));
        _states.Add(new WalkForwardState(animator));
        _states.Add(new WalkBackwardState(animator));
        _states.Add(new WalkLeftState(animator));
        _states.Add(new WalkRightState(animator));
        _states.Add(new StandupState(animator));
        _states.Add(new RunForwardState(animator));
        _states.Add(new RunBackwardState(animator));
    }
    
    public override void SetState<T>()
    {
        IState state = _states.First(x => x.GetType() == typeof(T));
        
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }
}