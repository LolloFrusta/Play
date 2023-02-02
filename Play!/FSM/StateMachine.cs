using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play_
{
    class StateMachine {   
        private Dictionary<StateEnum, State> states;
        private State current;
        private StateEnum inititalState;
        private bool firstTime;

        public StateMachine()
        {
            states = new Dictionary<StateEnum, State>();
            current = null;
            firstTime = true;
        }

        public void AddState(StateEnum key, State state)
        {
            states[key] = state; //check if already inserted
            state.SetStateMachine(this);
            //eventually on first Add State, set initial state, to avoid 'null state'
        }

        public void GoTo(StateEnum key)
        {
            if (current != null) current.OnExit();
            current = states[key];
            current.OnEnter();
        }

        public void Update()
        {
            if (firstTime)
            {
                firstTime = false;
                GoTo(inititalState);
            }

            current.Update();
        }

        public void SetFirstState(StateEnum initial)
        {
            inititalState = initial;
        }
    }
}
