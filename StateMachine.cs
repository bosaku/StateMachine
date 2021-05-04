using System;
using System.Collections.Generic;
using UnityEngine;

namespace SFI.AI
{
    public class StateMachine
    {
        private List<StateTransition> _stateTransitions = new List<StateTransition>();
        
        private List<StateTransition> _anyStateTransitions = new List<StateTransition>();

        public event Action<IState> OnStateChanged;
        
        private IState _currentState;
        public object CurrentState => _currentState;
        public IState PreviousState;

        public IState manuallySetState;
        public StateTransition manuallySetTransition;

        public void AddAnyTransition(IState to, Func<bool> condition)
        {
            var stateTransition = new StateTransition(null, to, condition);
            _anyStateTransitions.Add(stateTransition);
        }

        public StateTransition AddTransition(IState from, IState to, Func<bool> condition)
        {
            var stateTransition = new StateTransition(from, to, condition);
            _stateTransitions.Add(stateTransition);
            return stateTransition;
        }

        public void SetState(IState state)
        {
            if (_currentState == state) return;

            //if it doesn't exist, do nothing. else exit
            _currentState?.OnExit();
                
            _currentState = state;
           // Debug.Log($"Changed to {state}");
           // Debug.LogError($"Changed to {state}");
            _currentState.OnEnter();

            OnStateChanged?.Invoke(_currentState);
            PreviousState = _currentState;
        }
        
        public void Tick()
        {
            StateTransition transition = CheckForTransition();
            if (transition != null)
            {
                SetState(transition.To);
            }
            _currentState.Tick();
        }
        public void ManuallySetState(IState stateToTransitionTo)
        {
            if (stateToTransitionTo == _currentState) return;
            
            for (int i = 0; i < _stateTransitions.Count; i++)
            {
                if (_stateTransitions[i].From == _currentState)
                {
                    manuallySetState = stateToTransitionTo;
                    manuallySetTransition = AddTransition(_currentState, stateToTransitionTo, ()=> this.manuallySetState!=null && this.manuallySetTransition!=null);
                    break;
                }
            }
        }

        public StateTransition CheckForTransition()
        {
            foreach (var transition in _anyStateTransitions)
            {
                if (transition.Condition())
                    return transition;
            }
            
            foreach (var transition in _stateTransitions)
            {
                if (transition.From == _currentState && transition.Condition())
                {
                    return transition;
                }
            }
            
            return null;
        }
    }
}