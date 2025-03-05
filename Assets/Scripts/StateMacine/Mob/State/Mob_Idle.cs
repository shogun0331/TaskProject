using GB;

public class Mob_Idle : IMachine
{
    Mob _StateMacine;

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
        _StateMacine = (Mob)Data;
    }
}
