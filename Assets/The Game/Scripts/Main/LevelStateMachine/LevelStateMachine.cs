using System;
using System.Linq;
using UnityEngine;
using WindowsSystem;
using Zenject;
using IInitializable = Unity.VisualScripting.IInitializable;

namespace Shadow_Dominion.StateMachine
{
    public class LevelStateMachine : IStateMachine
    {
        private readonly MirrorLevelSyncer _mirrorLevelSyncer;

        public Action<IState> OnStateChanged;

        private IState _lastState;

        [Inject]
        public LevelStateMachine(
            WindowsController windowsController,
            CoroutineExecuter coroutineExecuter,
            InitializeStateUI initializeStateUI,
            LevelSO levelSo)
        {
            if (MirrorLevelSyncer.Instance == null)
                _mirrorLevelSyncer = new MirrorLevelSyncer(this);
            else
                MirrorLevelSyncer.Instance.Initialize(this);

            _states.Add(new GameplayState(windowsController));
            _states.Add(new FinishState(windowsController));
            _states.Add(new LevelInitializeState(
                windowsController,
                coroutineExecuter,
                initializeStateUI,
                this,
                levelSo));
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