using System.Collections.Generic;
using System.Linq;
using GB;
using UnityEngine;

public enum UnitState{Idle,Move,Dead}
public class Unit : StateMachine<UnitState>
{
    [SerializeField] string _id;
    [SerializeField] int _level = 1;
    Vector2 _movePosition;
    public Vector2 MovePosition{get{return _movePosition;}}

    
    Tile _tile;
    public Tile tile{get{return _tile;}}

    Status _status;
    public Status status{get{return _status;}}

    HashSet<SkillTableProb> _skillProbList;
    public HashSet<SkillTableProb> Skills{get{return _skillProbList;}}
    UnitTableProb _prob;

    [SerializeField] SPRAnimation _anim;

    public SPRAnimation anim{get{return _anim;}}


    
    void Awake()
    {
        Init();
    }

    void OnEnable()
    {
        ApplyPassive();
    }

    void OnDisable()
    {
        RemovePassive();
    }



    public Unit SetTile(Tile tile)
    {
        _tile = tile;
        return this;
    }

    
    public Unit SetData(int level)
    {
        _level = level;

        if(_prob == null)
        {
            
            var table = GameDataManager.GetTable<UnitTable>();
            var list = table.Datas.Where(v =>  v.UnitID == _id).ToList();
            if(list == null || list.Count == 0) 
            {
                Debug.LogError("UnitTable None ID : " + _id);
                return this;
            }

            _skillProbList = new HashSet<SkillTableProb>();

            var skillTable = GameDataManager.GetTable<SkillTable>();
            //담당 스킬정보 가져오기 같은 스킬 이름 있을경우 HashSet으로 방지
            for(int i =0; i< list.Count; ++i)
            {
                if(list[i].Level == _level) _prob = list[i];
                //유닛의 하위 레벨 스킬만 담기
                if(_level <=  list[i].Level)
                {
                    if(!string.IsNullOrEmpty(list[i].Abilitie1) && skillTable.ContainsKey(list[i].Abilitie1)) _skillProbList.Add(skillTable[list[i].Abilitie1]);
                    if(!string.IsNullOrEmpty(list[i].Abilitie2) && skillTable.ContainsKey(list[i].Abilitie2)) _skillProbList.Add(skillTable[list[i].Abilitie2]);
                    if(!string.IsNullOrEmpty(list[i].Abilitie3) && skillTable.ContainsKey(list[i].Abilitie3)) _skillProbList.Add(skillTable[list[i].Abilitie3]);
                    if(!string.IsNullOrEmpty(list[i].Abilitie4) && skillTable.ContainsKey(list[i].Abilitie4)) _skillProbList.Add(skillTable[list[i].Abilitie4]);
                }
            }

            //레벨이 테이블의 정의가 없는 경우
            if(_prob == null) 
            {
                Debug.LogError("UnitTable None Level : " + _id);
                return this;
            }

            _status = new Status
            {
                AD = _prob.ATK,
                A_SPD = _prob.A_SPD,
                A_DIST = _prob.A_DIST,
            };
        }
        return this;
    }

    public void SetMovePosition(Vector2 position)
    {
        _movePosition = position;
        ChangeState(UnitState.Move);
    }

    
    void ApplyPassive()
    {
           //패시브 발동
        if(_skillProbList != null)
        {
            foreach(var v in _skillProbList)
            {
                if(v.SkillType == "Passive")
                {

                }
            }
        }

    }

    void RemovePassive()
    {
           //패시브 발동 회수
        if(_skillProbList != null)
        {
            foreach(var v in _skillProbList)
            {
                if(v.SkillType == "Passive")
                {

                }
            }
        }

    }

}
