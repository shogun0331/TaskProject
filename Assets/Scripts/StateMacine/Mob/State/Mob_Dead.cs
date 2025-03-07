using GB;

public class Mob_Dead : IMachine
{
    Mob _mob;

    public void OnEnter()
    {
        ObjectPooling.Return(_mob.gameObject);
        
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
        _mob = (Mob)Data;
    }
}
