using System;
using System.Linq;
using Shadow_Dominion;
using Shadow_Dominion.InputSystem;
using Shadow_Dominion.StateMachine;
using UnityEngine;
using WindowsSystem;
using Zenject;

public class LevelStateMachine : IStateMachine, IInitializable, IDisposable
{
    private readonly InputHandler _inputHandler;

    private IState _lastState;

    [Inject]
    public LevelStateMachine(
        CursorService cursorService,
        WindowsController windowsController,
        InputHandler inputHandler,
        MirrorServer mirrorServer,
        CoroutineExecuter coroutineExecuter,
        MirrorPlayerSpawner mirrorPlayerSpawner,
        InitializeStateUI initializeStateUI)
    {
        _inputHandler = inputHandler;

        _states.Add(new GameplayState(windowsController, cursorService));
        _states.Add(new PauseState(windowsController, cursorService));
        _states.Add(new LevelInitializeState(
            windowsController, cursorService, mirrorPlayerSpawner, coroutineExecuter, MirrorLevel.Instance, initializeStateUI));
        
        _inputHandler.OnInputUpdate += HandleInput;
    }
    
    public void Dispose()
    {
        _inputHandler.OnInputUpdate -= HandleInput;
    }
    
    public void Initialize()
    {
        SetState<LevelInitializeState>();
    }

    private void HandleInput(InputData inputData)
    {
        if (!inputData.ESC)
            return;

        if (CurrentState.GetType() != typeof(PauseState))
        {
            _lastState = CurrentState;
            SetState<PauseState>();
            return;
        }
        
        SetState(_lastState);
    }

    public sealed override void SetState<T>()
    {
        IState state = _states.First(x => x.GetType() == typeof(T));

        CurrentState?.Exit();
        CurrentState = state;
        CurrentState.Enter();

        Debug.Log($"Current level state: {CurrentState.GetType()}");
    }

    public void SetState(IState state)
    {
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState.Enter();

        Debug.Log($"Current level state: {CurrentState.GetType()}");
    }
}