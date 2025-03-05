using System;
using System.Linq;
using Shadow_Dominion.InputSystem;
using UnityEngine;
using WindowsSystem;
using Zenject;

namespace Shadow_Dominion.StateMachine
{
    public class LevelStateMachine : IStateMachine, IInitializable
    {
        private readonly InputHandler _inputHandler;
        private readonly MirrorLevelSyncer _mirrorLevelSyncer;

        public Action<IState> OnStateChanged;

        private IState _lastState;

        [Inject]
        public LevelStateMachine(
            CursorService cursorService,
            WindowsController windowsController,
            InputHandler inputHandler,
            MirrorServer mirrorServer,
            CoroutineExecuter coroutineExecuter,
            MirrorPlayerSpawner mirrorPlayerSpawner,
            InitializeStateUI initializeStateUI,
            LevelSO levelSo)
        {
            _inputHandler = inputHandler;
            _mirrorLevelSyncer = new MirrorLevelSyncer(this);

            _states.Add(new GameplayState(windowsController, cursorService));
            _states.Add(new LevelInitializeState(
                windowsController,
                cursorService,
                mirrorPlayerSpawner,
                coroutineExecuter,
                initializeStateUI,
                this,
                levelSo));
        }

        public void Initialize()
        {
            SetState<LevelInitializeState>();
        }

        public sealed override void SetState<T>()
        {
            IState state = _states.First(x => x.GetType() == typeof(T));

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
            
            OnStateChanged?.Invoke(state);

            Debug.Log($"Current level state: {CurrentState.GetType()}");
        }
        
        public void SetState(string stateName)
        {
            IState state = _states.First(x => x.GetType().ToString() == stateName);

            if (CurrentState == state)
                return;

            CurrentState?.Exit();
            CurrentState = state;
            CurrentState.Enter();
            
            Debug.Log($"Current player state: {CurrentState.GetType()}");
        }
    }
}