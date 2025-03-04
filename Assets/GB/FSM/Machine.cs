using UnityEngine;

namespace GB
{
    public  interface IMachine
    {
        public void SetMachine(IStateMachineMachine Data);
        public void OnEnter();
        public void OnUpdate();
        public void OnExit();
        public void OnEvent(string eventName);

    }
}
