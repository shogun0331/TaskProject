using GB;

public class Unit_Dead : IMachine
{
    Unit _StateMacine;

    public void OnEnter()
    {
        
    }
    
    public void OnUpdate()
    {

    }

    public void OnExit()
    {
        
    }


    public void OnEvent(string eventName)
    {
        
    }

    public void SetMachine(IStateMachineMachine Data)
    {
        _StateMacine = (Unit)Data;
    }
}
