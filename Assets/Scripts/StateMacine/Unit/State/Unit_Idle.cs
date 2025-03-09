using GB;
using UnityEngine;

public class Unit_Idle : IMachine
{
    Unit _unit;
    bool _isAtk = false;
    float _atkTime;
    float _atkDelay;

    Transform _target;

    public void OnEnter()
    {
        OnEndAttack();
        _unit.anim.EndEvent.AddListener(OnEndAttack);
        _unit.anim.TriggerEvent.AddListener(OnTriggerEvent);
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
        _unit.anim.TriggerEvent.RemoveListener(OnTriggerEvent);
    }

    public void OnEvent(string eventName)
    {

    }

    void Fire()
    {
        _target  = FindTarget();

        if(_target != null)
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

    void OnTriggerEvent(TriggerData data)
    {
       Attack();

    }

    public int GetPercent()
    {
        if(_unit.rank == UnitRank.C || _unit.rank == UnitRank.B) return _unit.player.Levels["C"];
        else if( _unit.rank == UnitRank.A) return _unit.player.Levels["A"];
        else if( _unit.rank == UnitRank.S) return _unit.player.Levels["S"];
        return 1;
    }

    void Attack()
    {
         _target  = FindTarget();
        if(_target == null) return;
        if(!_target.gameObject.activeSelf) return;

        if(Skill()) return;


        var oj = ObjectPooling.Create("ProjectTile/"+_unit.projectTileID);
        if(oj == null) return;


        
        Hit hit = new Hit
        {
            player = _unit.player,
            AD = _unit.status.AD * GetPercent()
        };

        oj.transform.position = _unit.transform.position;
        oj.GetComponent<ProjectTile>().SetTarget(_target).SetHit(hit).Play();
    }

    bool Skill()
    {
        foreach(var v in _unit.Skills)
        {
            int rand = Random.Range(0,100);

            if(rand <= v.Chance )
            {
                var oj = ObjectPooling.Create("Skill/ProjectTile/"+v.ID);
                oj.transform.position = _unit.transform.position;

                Hit hit = new Hit
                {
                    player = _unit.player,
                    AD = _unit.status.AD *  GetPercent(),
                    AP = _unit.status.AD *  GetPercent(),
                    skillProb = v
                };


                var p = oj.GetComponent<ProjectTile>();
                p.SetHit(hit).SetTarget(_target.position).Play();
                
                return true;
            }
        }

        return false;

    }

    void OnEndAttack()
    {
        _atkDelay = 1f / _unit.status.A_SPD;
        _isAtk = false;
        _unit.anim.Play("Idle");
    }

    public void SetMachine(IStateMachineMachine Data)
    {
        _unit = (Unit)Data;
    }
}
