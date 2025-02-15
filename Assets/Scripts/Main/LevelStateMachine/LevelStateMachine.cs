using System;
using System.Linq;
using Shadow_Dominion.InputSystem;
using Shadow_Dominion.StateMachine;
using UnityEngine;
using WindowsSystem;
using Zenject;

public class LevelStateMachine : IStateMachine, IDisposable
{
    private readonly InputHandler _inputHandler;

    [Inject]
    public LevelStateMachine(
        CursorService cursorService,
        WindowsController windowsController,
        InputHandler inputHandler)
    {
        _inputHandler = inputHandler;

        _states.Add(new GameplayState(windowsController, cursorService));
        _states.Add(new PauseState(windowsController, cursorService));

        SetState<GameplayState>();

        _inputHandler.OnInputUpdate += HandleInput;
    }

    private void HandleInput(InputData inputData)
    {
        if (inputData.ESC)
        {
            if (CurrentState.GetType() == typeof(GameplayState))
                SetState<PauseState>();
            else
                SetState<GameplayState>();
        }
    }

    public void Dispose()
    {
        _inputHandler.OnInputUpdate -= HandleInput;
    }

    public sealed override void SetState<T>()
    {
        IState state = _states.First(x => x.GetType() == typeof(T));
        
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState.Enter();
        
        Debug.Log($"Current level state: {CurrentState.GetType()}");
    }
}