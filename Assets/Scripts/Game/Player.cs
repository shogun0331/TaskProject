using System.Collections.Generic;
using GB;
using UnityEngine;

[System.Serializable]
public class Player 
{
    int _id;
    public int ID{get{return _id;}}

    int _gold = 0;
    //골드
    public int Gold{get{return _gold;}}
    
    int _luck;
    //행운석
    public int Luck{get{return _luck;}}

    //소환 가격
    int _summonPrice;
    public int SummonPrice{get{return _summonPrice;}}

    //유닛 최대 수
    int _maxUnitCount = 20;
    public int MaxUnitCount{get{return _maxUnitCount;}}


    //강화 레벨 관리
    Dictionary<string,int> _dictUpradeLevel;
    //소환 할수 있는 유닛 관리
    Dictionary<UnitRank,List<UnitData>> _dictUnitIDData;

    //게임보드
    Board _board;

    //소환 테이블
    SummonTable _summonTable;

    bool _isSummon;

    public void Init(Board board,List<UnitData> units ,int playerID)
    {
        _id = playerID;

        _summonPrice = 20;
        _maxUnitCount = 20;

        _summonTable = GameDataManager.GetTable<SummonTable>();
        
        _dictUpradeLevel = new Dictionary<string, int>();
        _dictUpradeLevel["RANKC-A"] = 1;
        _dictUpradeLevel["RANK-SS"] = 1;
        _dictUpradeLevel["Summon"] = 1;


        _dictUnitIDData = new Dictionary<UnitRank, List<UnitData>>();
        _dictUnitIDData[UnitRank.C] = new List<UnitData>();
        _dictUnitIDData[UnitRank.B] = new List<UnitData>();
        _dictUnitIDData[UnitRank.A] = new List<UnitData>();
        _dictUnitIDData[UnitRank.S] = new List<UnitData>();
        _dictUnitIDData[UnitRank.SS] = new List<UnitData>();

        foreach(var v in units) _dictUnitIDData[v.Rank].Add(v);

        _board = board;
        _gold = 220;
        _luck = 0;

        if(ID == 0) ODataBaseManager.Set("GOLD",_gold);
        if(ID == 0) ODataBaseManager.Set("SUMMON_GOLD",_summonPrice);
        
        
    }

    public void Summon()
    {
        // if(!CheckSummon()) return;
        
        // _gold -= _summonPrice;
        //유닛 수 체크 
        _summonPrice += DEF.SUMMON_ADD_GOLD;
        if(ID == 0) 
        {
            ODataBaseManager.Set("SUMMON_GOLD",_summonPrice);
            ODataBaseManager.Set("GOLD",_gold);
            CheckSummonGold();
        }

        _board.AddUnit(this,CreateUnit(GetRandomUnitData()));
    }

    public bool CheckSummonGold()
    {
        if(Gold < SummonPrice) 
        {
            _isSummon = false;
            if(ID == 0)  ODataBaseManager.Set("SUMMON_ACTIVE",_isSummon);
            return false;
        }
        _isSummon = true;
        return true;
    }

    public bool CheckSummon()
    {
        if(_board.GetUnitCount(ID) >= _maxUnitCount) return false;
        if(!CheckSummonGold()) return false;
        
        return true;
    }

    public void Merge(Tile tile)
    {
        if(tile.Rank == UnitRank.A) return;

        int irank = (int)tile.Rank;
        irank ++;

        tile.ClearUnits();
        var unit = CreateUnit(GetRandomUnitData((UnitRank)irank));
        unit.transform.SetParent(_board.transform);
        int idx = _board.ContainsUnitTeam(unit.ID,ID);
        if(idx == -1)
            tile.AddUnit(unit);
        else 
            _board.AddUnit(this,unit);
    }


      /// <summary>
    /// 유닛 생성
    /// </summary>
    /// <param name="unitID">유닛 ID</param>
    /// <returns>유닛</returns>
    Unit CreateUnit(UnitData data)
    {
        var unit = ObjectPooling.Create("Unit/"+ data.ID,5).GetComponent<Unit>();
        unit.SetData(this,data.Level);
        return unit;
    }

    public void DeadMob(GameObject mob, MobTableProb prob)
    {
        _board.RemoveMob(mob);
        AddGold(prob.Gold);
    }

    public void AddGold(int gold)
    {
        _gold += gold;
        CheckSummonGold();
        if(ID == 0)  ODataBaseManager.Set("GOLD",_gold);
    }

    UnitData GetRandomUnitData()
    {
        var prob = _summonTable[_dictUpradeLevel["Summon"].ToString()];
        //확률 
        int[] percents = new int[3] {prob.C_Weight,prob.B_Weight,prob.A_Weight,};
        int randRank = RandomUtil.NextWeightedInd(percents);
        if(randRank == 0)       return _dictUnitIDData[UnitRank.C][Random.Range(0,_dictUnitIDData[UnitRank.C].Count)];
        else if(randRank == 1)  return _dictUnitIDData[UnitRank.B][Random.Range(0,_dictUnitIDData[UnitRank.B].Count)];
        else                    return _dictUnitIDData[UnitRank.A][Random.Range(0,_dictUnitIDData[UnitRank.A].Count)];
    }

    UnitData GetRandomUnitData(UnitRank rank)
    {
        return _dictUnitIDData[rank][Random.Range(0,_dictUnitIDData[rank].Count)];
    }
    
}
