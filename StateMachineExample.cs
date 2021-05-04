using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SFI.GameStates
{
    /// <summary>
    /// This state machine is about the simplest state transition / requirement that I could imagine.
    /// If you want to make more / different state machines,
    /// just make this a base class and override the Awake() method
    /// with a new set of state definitions.
    /// </summary>
    public class StateMachineExample : MonoBehaviour
    {
        public static event Action<IState> OnGameStateChanged;

        protected StateMachine _stateMachine;
        
        //Yes I am using a Singleton. They are still quite useful!
        public static StateMachineExample _instance;
        
        public IState returnToState;
        public string currentStateName;
        
        public Type CurrentStateType => _stateMachine.CurrentState.GetType();
        public IState LastStateType => _stateMachine.PreviousState;

        private void Awake()
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

            //create your states here
            //these states define state behaviors and would probably not be in this class. 
            var loadingState = new ExampleState();
            var secondState = new SecondState(this); // pass a Monobehavior to a state's constructor to use a coroutine
            var thirdState = new ThirdState();
            
            //Set the first state
            _stateMachine.SetState(loadingState); 
            
            //add transitions between states here
            //the first transition requires a scene called IntroScene to be loaded - when it is, the system will automatically change to the second state
            _stateMachine.AddTransition(loadingState, secondState, ()=> SceneManager.GetActiveScene().name == "IntroScene" );
            //this second transition relies on the state change to occur from within the state
            _stateMachine.AddTransition(secondState, thirdState, ()=> secondState.Finished);
            //another example of a transition might be to test a Singleton
            //_stateMachine.AddTransition(secondState, thirdState, ()=> MySingletonFromAnotherSystem.IsFinished);
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

    //Do all the things here!
    internal class ExampleState : IState
    {
        //called every Update(). You can add a FixedUpdateTick() if you need it
        public void Tick()
        {
            
        }

        //initialization code
        public void OnEnter()
        {
            
        }

        //clean up code
        public void OnExit()
        {
            
        }
    }
    
    //Do more things here!
    internal class SecondState : IState
    {
        public SecondState(StateMachineExample controller)
        {
            controller.StartCoroutine(YouCanAddCoroutines());
        }

        private IEnumerator YouCanAddCoroutines()
        {
            yield return null;
            Finished = true;
        }
        
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }

        public bool Finished { get; set; }
    }
    
    internal class ThirdState : IState
    {
        public void Tick()
        {
            
        }

        public void OnEnter()
        {
            
        }

        public void OnExit()
        {
            
        }
    }
}
