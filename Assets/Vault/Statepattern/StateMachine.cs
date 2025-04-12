namespace Vault
{
    public class StateMachine<T>
    {
        private T owner;
        private IState<T> currentState;
        private IState<T> previousState;

        public StateMachine(T owner)
        {
            this.owner = owner;
            this.currentState = null;
            this.previousState = null;
        }

        public void ChangeState(IState<T> newState)
        {
            if (currentState != null)
            {
                currentState.Exit(owner);
            }

            previousState = currentState;
            currentState = newState;

            if (currentState != null)
            {
                currentState.Enter(owner);
            }
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.Execute(owner);
            }
        }

        public void RevertToPreviousState()
        {
            ChangeState(previousState);
        }

        public IState<T> GetCurrentState()
        {
            return currentState;
        }

        public IState<T> GetPreviousState()
        {
            return previousState;
        }
    }
}

