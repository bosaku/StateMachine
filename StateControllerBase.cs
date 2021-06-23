using System;
using System.Collections;
using UnityEngine;

namespace SFI.GameStates
{
    /// <summary>
    /// This state machine is about the simplest state transition / requirement that I could imagine.
    /// If you want to make more / different state machines,
    /// just make this a base class and override the Awake() method
    /// with a new set of state definitions.
    /// </summary>
    public class StateControllerBase : MonoBehaviour
    {
        public static event Action<IState> OnGameStateChanged;

        protected StateMachine _stateMachine;
        
        //Yes I am using a Singleton. They are still quite useful!
        public static StateControllerBase _instance;
        
        public IState returnToState;
        public string currentStateName;
        
        public Type CurrentStateType => _stateMachine.CurrentState.GetType();
        public IState LastStateType => _stateMachine.PreviousState;

        protected void Init()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            DontDestroyOnLoad(gameObject);
            _stateMachine = new StateMachine();
            
            //Subscribe to OnGameStateChanged elsewhere if you want to see when a state changes as well as get access to information in the state
            _stateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);
            _stateMachine.OnStateChanged += state => StateChanged(state);
        }
        
        private void StateChanged(IState state)
        {
            currentStateName = state.ToString();
        }

        public void Update()
        {
            _stateMachine.Tick();
        }
    }
}
