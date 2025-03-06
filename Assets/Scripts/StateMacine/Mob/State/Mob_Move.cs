using GB;
using UnityEngine;

public class Mob_Move : IMachine
{
    Mob _mob;

    public void OnEnter()
    {
        _mob.SetAnimation("Move");
    }
    
    public void OnUpdate()
    {
        if(_mob.MovePaths == null ||_mob.MovePaths.Length == 0)
        {
            _mob.ChangeState(MobState.Idle);
            return;
        } 

        Vector2 position = _mob.transform.position;

        float dt = GBTime.GetDeltaTime(DEF.T_GAME);
        // 경로에 따라 이동
        if (_mob.moveIdx < _mob.MovePaths.Length)
        {
            Vector2 targetPosition = _mob.MovePaths[_mob.moveIdx];
            Vector2 direction = (targetPosition - position).normalized;
            if(direction.x < 0) _mob.SetFlipX(false);
            else _mob.SetFlipX(true);

            float speed = _mob.status.M_SPD - (_mob.status.M_SPD * _mob.MoveSlow);
            if(speed < 0) speed = 0;

            position += direction * _mob.status.M_SPD * 2 * dt;
            if(Vector2.Distance(position,targetPosition) < 0.1f) 
            {
                _mob.moveIdx++;
                position = targetPosition;
            }
            
            //HP bar 이동
            if(_mob.hpView != null)
            {
                Vector2 pos = position;
                pos.y += 0.45f;
               _mob.hpView.transform.position =  pos;
            }
        }
        else
        {
            _mob.moveIdx = 0;
        }

        _mob.transform.position = position;

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
