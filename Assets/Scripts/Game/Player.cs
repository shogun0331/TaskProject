using System.Collections.Generic;
using System.Linq;
using GB;
using Unity.VisualScripting;
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

    public IReadOnlyDictionary<UnitRank,List<UnitData>> UnitIDDatas{get{return _dictUnitIDData;}} 

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
        _dictUpradeLevel["RANK-S"] = 1;
        _dictUpradeLevel["Summon"] = 1;

        _dictUnitIDData = new Dictionary<UnitRank, List<UnitData>>();
        _dictUnitIDData[UnitRank.C] = new List<UnitData>();
        _dictUnitIDData[UnitRank.B] = new List<UnitData>();
        _dictUnitIDData[UnitRank.A] = new List<UnitData>();
        _dictUnitIDData[UnitRank.S] = new List<UnitData>();
        
        foreach(var v in units) _dictUnitIDData[v.Rank].Add(v);

        _board = board;
        _gold = 220;
        _luck = 0;

        if(ID == 0) 
        {
            ODataBaseManager.Set("GOLD",_gold);
            ODataBaseManager.Set("SUMMON_GOLD",_summonPrice);
            ODataBaseManager.Set("Player",this);
        }
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

    public void SummonMythMerge(string unitName)
    {
        if(!CheckMythSummon(unitName)) return;

        var priceUnits = GetTableUnits(unitName);
        // int totalCount = 0;
        // foreach(var v in priceUnits) totalCount += v.Value;
        var tiles = _board.GetTiles(ID);

        foreach(var v in priceUnits)
        {
            var tileList = tiles.Where(t=>t.UnitID == v.Key).ToList();
            tileList.Sort((a,b)=> a.UnitCount.CompareTo(b.UnitCount));
            int cnt = v.Value;

            for(int i = 0; i< tileList.Count; ++i)
            {
                if(cnt == 0) break;
                cnt -= tileList[i].RemoveUnit(cnt);
            }
        }

        CreteUnit(unitName);

    }

    

    public void CreteUnit(string unitName)
    {
        Debug.Log(unitName);
        Debug.Log("0");
        UnitData unitData = null;
        foreach(var v  in _dictUnitIDData)
            unitData = v.Value.FirstOrDefault(v=>v.ID == unitName);

        

        if(unitData == null) return;

        Debug.Log("1");
        

        if(unitData.Rank == UnitRank.A ||unitData.Rank == UnitRank.S)
        Presenter.Send("Summon","Rank",unitData.Rank);

        var unit = CreateUnit(unitData);
        unit.transform.SetParent(_board.transform);
        _board.AddUnit(this,unit);
    }

     public void GachaUnit(string id)
     {
        UnitData data = null;

        if(id == "A")
           data = Get_UnitRandom_B_C();
        else if(id == "B")
            data = Get_UnitRandom_A();
        else
            data = Get_UnitRandom_S();

        var unit = CreateUnit(data);
        unit.transform.SetParent(_board.transform);
        _board.AddUnit(this,unit);
      
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
        {
            tile.AddUnit(unit);
            CreateCreateFX().transform.position = tile.Position;
        }
        else 
        {
            var mTile = _board.GetTile(idx,ID);
            mTile.AddUnit(unit,tile.Position);
            CreateCreateFX().transform.position = tile.Position;
        }
    }

    GameObject CreateCreateFX()
    {
         var eff = ObjectPooling.Create(RES_PREFAB.FX_UnitCreateFX,5);    
        return eff;
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


    UnitData Get_UnitRandom_B_C()
    {
        int rand = Random.Range(0,100);
        if(rand > 50)
            return _dictUnitIDData[UnitRank.C][Random.Range(0,_dictUnitIDData[UnitRank.C].Count)];
        else
            return _dictUnitIDData[UnitRank.B][Random.Range(0,_dictUnitIDData[UnitRank.B].Count)];
    }

    UnitData Get_UnitRandom_A()
    {
        return _dictUnitIDData[UnitRank.A][Random.Range(0,_dictUnitIDData[UnitRank.A].Count)];
            
    }

    UnitData Get_UnitRandom_S()
    {
        return _dictUnitIDData[UnitRank.S][Random.Range(0,_dictUnitIDData[UnitRank.S].Count)];
            
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

    /// <summary>
    /// 신화 등급 소환 가능 한가 체크
    /// </summary>
    /// <param name="id">신화 ID</param>
    /// <returns></returns><summary>    
    public bool CheckMythSummon(string unitName)
    {
        var table = GameDataManager.GetTable<MythTable>();

        // 유저의 신화 등급의 Unlock 체크 
        var unit = _dictUnitIDData[UnitRank.S].FirstOrDefault(v=>v.ID == unitName);
        if(unit == null) return false;

        // 테이블에 정의 되어있는가 체크
        if(table.ContainsKey(unitName))
        {
            Dictionary<string,int> data = GetTableUnits(unitName);
           
            //갯수가 부족한가 체크
            foreach(var v in data)
            {
                if(v.Value > GetUnitCount(v.Key)) return false;
            }
           
            return true;

        }

        return false;
    }

    //유닛의 갯수
    public int GetUnitCount(string id)
    {
        int length = _board.GetTileLength();
        int cnt = 0;
        for(int i = 0; i< length; ++i)
        {
           var tile = _board.GetTile(i,_id);
           if(tile.UnitID == id) cnt += tile.UnitCount;
        }

        return cnt;
    }

    Dictionary<string, int> GetTableUnits(string unitName)
    {
        var table = GameDataManager.GetTable<MythTable>();
        Dictionary<string, int> data = new Dictionary<string, int>();
        if (!string.IsNullOrEmpty(table[unitName].A_ID))
        {
            data[table[unitName].A_ID] = 1;
        }

        if (!string.IsNullOrEmpty(table[unitName].B_ID))
        {
            string id = table[unitName].B_ID;
            if (data.ContainsKey(id)) data[id]++;
            else data[id] = 1;
        }

        if (!string.IsNullOrEmpty(table[unitName].C_ID))
        {
            string id = table[unitName].C_ID;
            if (data.ContainsKey(id)) data[id]++;
            else data[id] = 1;
        }

        if (!string.IsNullOrEmpty(table[unitName].D_ID))
        {
            string id = table[unitName].D_ID;
            if (data.ContainsKey(id)) data[id]++;
            else data[id] = 1;
        }

        return data;

    }

}
