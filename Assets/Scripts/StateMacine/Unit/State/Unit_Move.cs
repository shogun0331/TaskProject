
using GB;
using UnityEngine;

public class Unit_Move : IMachine
{
    Unit _unit;

    float _moveSpeed = 2;

    public void OnEnter()
    {
        _unit.anim.Play("Move");
    }
    
    public void OnUpdate()
    {
        float dt = GBTime.GetDeltaTime(DEF.T_GAME);
        Vector2 dir = _unit.MovePosition - (Vector2)_unit.transform.position;

        Vector3 pos = _unit.MovePosition;
        pos.z = pos.y;

        
        _unit.transform.Translate(dir.normalized * dt * _moveSpeed);
        float dist = Vector2.Distance(_unit.transform.position,_unit.MovePosition);

        if(dist < 0.1f)
        {
            
            _unit.transform.position =  _unit.MovePosition;
            _unit.ChangeState(UnitState.Idle);
        }
    }

    public void OnExit()
    {
        
    }


    public void OnEvent(string eventName)
    {
        
    }

    public void SetMachine(IStateMachineMachine Data)
    {
        _unit = (Unit)Data;
    }
}
