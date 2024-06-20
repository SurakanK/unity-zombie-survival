
namespace StatePatternInUnity
{
    public class StateMachine
    {
        public IState currentState;
        
        public void Initialize(IState startingState)
        {
            currentState = startingState;
            startingState.Initialize(this);
        }

        public void ChangeState(IState newState)
        {
            currentState.OnEnded();

            currentState = newState;
            newState.Initialize(this);
        }
    }
}