using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private List<StateTransition> _stateTransitions = new List<StateTransition>();
    private List<StateTransition> _anyStateTransitions = new List<StateTransition>();
    
    private IState _currentState;
    public IState CurrentState => _currentState;
    public event Action<IState> OnStateChanged;

    public void AddAnyTransition(IState to, Func<bool> condition) {
        var stateTransition = new StateTransition(null, to, condition);

        // example of AnyTransition is "anything ... to Dead"
        _anyStateTransitions.Add(stateTransition);
    }

    public void AddTransition(IState from, IState to, Func<bool> condition)
    {
        // example of Transition is "idle to attack when within range"
        var stateTransition = new StateTransition(from, to, condition);
        _stateTransitions.Add(stateTransition);
    }

    public void SetState(IState state)
    {
        if (_currentState == state)
            return;
        
        _currentState?.OnExit();
        _currentState = state;
        // Debug.Log($"{this} changed to state {state}");
        _currentState.OnEnter();
        
        OnStateChanged?.Invoke(_currentState);
    }

    public void Tick()
    {
        StateTransition transition = CheckForTransition();
        if (transition != null) {
            SetState(transition.To);
        }
        _currentState.Tick();
    }

    private StateTransition CheckForTransition()
    {
        // check 'any state' transitions first
        foreach (var transition in _anyStateTransitions) {
            if (transition.Condition()) {
                return transition;
            }
        }
        
        // now check state-specific transitions
        foreach (var transition in _stateTransitions) {
            if (transition.From == _currentState && transition.Condition()) {
                return transition;
            }
        }
        
        return null;
    }
}
