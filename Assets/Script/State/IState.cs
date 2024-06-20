using System.Collections;

namespace StatePatternInUnity
{

    public abstract class IState
    {
        public void Initialize(StateMachine stateMachine)
        {
            OnInitialized();
            OnActive();
        }

        public virtual void OnInitialized()
        {

        }

        public virtual void OnActive()
        {

        }

        public virtual IEnumerator DoLogic()
        {
            yield return null;
        }

        public virtual void OnEnded()
        {

        }
    }
}