using GB;
using UnityEngine;

public class Unit_Idle : IMachine
{
    Unit _unit;
    bool _isAtk = false;
    float _atkTime;
    float _atkDelay;

    public void OnEnter()
    {
        OnEndAttack();
        _unit.anim.EndEvent.AddListener(OnEndAttack);
    }

    public void OnUpdate()
    {
        if (_isAtk) return;

        _atkTime += GBTime.GetDeltaTime(DEF.T_GAME);
        if (_atkTime > _atkDelay)
        {
            _atkTime = 0;
            Fire();
        }
    }

    public void OnExit()
    {
        _unit.anim.EndEvent.RemoveListener(OnEndAttack);
    }

    public void OnEvent(string eventName)
    {

    }

    void Fire()
    {
        var target  = FindTarget();

        if(target != null)
        {
            _isAtk = true;
            _unit.anim.Play("Attack");
        }
    }

    Transform FindTarget()
    {
        if(_unit.tile == null) return null;

        Vector2 position = _unit.tile.Position;
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(position, _unit.status.A_DIST);
        float dist = Mathf.Infinity;

        Transform nearTarget = null;
        // 찾은 콜라이더 처리
        foreach (Collider2D coll in hitColliders)
        {
            Mob mob = coll.GetComponent<Mob>();
            if(mob != null)
            {
                float distance = Vector2.Distance(position, mob.transform.position);
                if (distance < dist)
                {
                    dist = distance;
                    nearTarget = mob.transform;
                }
            }
        }

        return nearTarget;
    }

    void OnEndAttack()
    {
        _atkDelay = 1f / _unit.status.A_SPD;
        _isAtk = false;
        _atkTime = 0;
        _unit.anim.Play("Idle");
    }

    public void SetMachine(IStateMachineMachine Data)
    {
        _unit = (Unit)Data;
    }
}
