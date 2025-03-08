using System.Security.Cryptography.X509Certificates;
using System.Collections.Generic;
using System.Linq;
using GB;
using UnityEngine;

public class Tile
{
    public string ID;
    public Vector2 Position;
    List<Unit> _unitList = new List<Unit>();


    // 결합이 가능한 상태 인가?
    public bool IsMerge
    {
        get
        {
            if (_unitList == null || !Max) return false;
            return true;
        }
    }

    public int UnitCount
    {
        get
        {
            if (_unitList == null) return 0;
            return _unitList.Count;
        }
    }

    public bool Max
    {
        get
        {
            if (_unitList == null) return false;
           
            int weight = 0;
            for (int i = 0; i < _unitList.Count; ++i) weight += _unitList[i].weight;
            return weight >= 3;
        }
    }
    public string UnitID
    {
        get
        {
            if (_unitList == null || _unitList.Count == 0 || _unitList[0] == null) return null;
            return _unitList[0].ID;
        }
    }

    public float AttackDist
    {
        get
        {
            if (_unitList == null || _unitList.Count == 0 || _unitList[0] == null) return 0;
             return _unitList[0].Disance;
        }
    }

    public UnitRank Rank
    {
        get
        {
            if (_unitList == null || _unitList.Count == 0 || _unitList[0] == null) return UnitRank.C;
             return _unitList[0].rank;
        }

    }

    public void AddUnit(Unit unit)
    {
        _unitList.Add(unit);
        unit.transform.position = Position;
        unit.SetTile(this);
        unit.SetMovePosition(GetUnitPosition(_unitList.Count - 1));

    }

    public void AddUnit(Unit unit, Vector2 position)
    {
        _unitList.Add(unit);
        unit.transform.position = position;
        unit.SetTile(this);
        unit.SetMovePosition(GetUnitPosition(_unitList.Count - 1));
    }
    public bool RemoveUnit()
    {
        if(_unitList == null || _unitList.Count == 0) return true;

        var unit = _unitList[_unitList.Count - 1];
        ObjectPooling.Return(unit.gameObject);
        _unitList.Remove(unit);

        return false;
    }

    public int RemoveUnit(int count)
    {
        if(_unitList == null || _unitList.Count == 0) return 0;

        for(int i = 0; i< count; ++i)
        {
            bool isRemove = RemoveUnit();
            if(!isRemove)
            return i+1;
        }
        
        return 0;
    }


    // 결합
    public bool Merge()
    {
        if (!IsMerge) return false;
        //결합
        _unitList[0].player.Merge(this);
        return true;
    }

    public void SellUnit()
    {

    }

    public void ClearUnits()
    {
        for (int i = 0; i < _unitList.Count; ++i) ObjectPooling.Return(_unitList[i].gameObject);
        _unitList.Clear();
    }

    public void SwapUnits(Tile tile)
    {
        var list = tile._unitList;
        tile._unitList = _unitList;
        _unitList = list;
        tile.Refresh();
        Refresh();
    }

    Vector2 GetUnitPosition(int index)
    {
        Vector2 pos = Position;
        if (_unitList[index].weight == 1)
        {
            if (index == 0)
            {
                pos.x += 0.16f;
                pos.y += 0.19f;
            }
            else if (index == 1)
            {
                pos.x -= 0.16f;
                pos.y += 0.19f;
            }
            else
            {
                pos.y += 0.07f;
            }
        }

        return pos;
        

    }

    public void Refresh()
    {
        if (UnitCount == 0) return;

        for (int i = 0; i < _unitList.Count; ++i)
        {
            _unitList[i].SetTile(this);
            _unitList[i].SetMovePosition(GetUnitPosition(i));
        }
    }



}
