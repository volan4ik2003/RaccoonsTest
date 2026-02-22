using _Game.Scripts.Infrastructure.States;
using _Game.Scripts.Infrastructure.States.ConcreteStates;
using System;
using System.Collections.Generic;

public class GameStateMachine
{
    private readonly Dictionary<Type, IExitableState> _states;
    private IExitableState _activeState;

    public GameStateMachine(
        LoadProgressState loadProgressState,
        LoadLevelState loadLevelState,
        GameLoopState gameLoopState)
    {
        _states = new Dictionary<Type, IExitableState>
        {
            [typeof(LoadProgressState)] = loadProgressState,
            [typeof(LoadLevelState)] = loadLevelState,
            [typeof(GameLoopState)] = gameLoopState
        };
    }

    public void Enter<TState>() where TState : class, IState
    {
        TState state = ChangeState<TState>();
        state.Enter();
    }

    public void Enter<TState, TPayLoad>(TPayLoad payLoad)
        where TState : class, IPayLoadedState<TPayLoad>
    {
        TState state = ChangeState<TState>();
        state.Enter(payLoad);
    }

    private TState GetState<TState>() where TState : class, IExitableState =>
        _states[typeof(TState)] as TState;

    private TState ChangeState<TState>() where TState : class, IExitableState
    {
        _activeState?.Exit();
        TState state = GetState<TState>();
        _activeState = state;
        return state;
    }
}