using System;
using System.Collections;
using SFI.AI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStates
{
    /// <summary>
    /// This state machine is about the simplest state transition / requirement that I could imagine.
    /// If you want to make more / different state machines,
    /// just make this a base class and override the Awake() method
    /// with a new set up state definitions.
    /// </summary>
    public class StateMachineExample : MonoBehaviour
    {
        public static event Action<IState> OnGameStateChanged;

        //Yes I am using a Singleton. They are still quite useful!
        protected StateMachine _stateMachine;
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
            _stateMachine.OnStateChanged += state => OnGameStateChanged?.Invoke(state);
            _stateMachine.OnStateChanged += state => StateChanged(state);

            //create your states here
            var loadingState = new ExampleState();
            var secondState = new SecondState(this); // pass a Monobehavior to a state's constructor to use a coroutine
            var thirdState = new ThirdState();
            
            //add transitions between states here
            _stateMachine.AddTransition(loadingState, secondState, ()=> SceneManager.GetActiveScene().name == "IntroScene" );
            _stateMachine.AddTransition(secondState, thirdState, ()=> secondState.Finished);
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