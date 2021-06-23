using System.Collections;
using UnityEngine.SceneManagement;

namespace SFI.GameStates
{
    public class StateController : StateControllerBase
    {
        void Awake()
        {
            Init();
            
            //create your states here.
            var loadingState = new ExampleState();
            var secondState = new SecondState(this); // pass a Monobehavior to a state's constructor to use a coroutine
            var thirdState = new ThirdState();

            //add transitions between states here
            _stateMachine.AddTransition(loadingState, secondState, 
                ()=> SceneManager.GetActiveScene().name == "IntroScene" );
            //this second transition relies on the state change to occur from within the state
            _stateMachine.AddTransition(secondState, thirdState, 
                ()=> secondState.Finished);
            //another example of a transition might be to test a Singleton
            //_stateMachine.AddTransition(secondState, thirdState, ()=> MySingletonFromAnotherSystem.IsFinished);
            
            //Set the first state. You can pass any state in as the starting state.
            _stateMachine.SetState(_stateMachine.GetStartingTransition());
        }
    }
    
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
        public SecondState(StateControllerBase controllerBase)
        {
            controllerBase.StartCoroutine(YouCanAddCoroutines());
        }

        private IEnumerator YouCanAddCoroutines()
        {
            yield return null;
            Finished = true;
        }
        
        public void Tick()
        {
            //is animation finished?
            Finished = true;
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
