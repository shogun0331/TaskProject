using System.Collections.Generic;
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
            if (_unitList == null || _unitList.Count < 3) return false;
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

    public bool UnitMax
    {
        get
        {
            if (_unitList == null) return false;
            return _unitList.Count == 3;
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

    public void AddUnit(Unit unit)
    {
        _unitList.Add(unit);
        unit.transform.position = Position;
        unit.SetMovePosition(GetUnitPosition(_unitList.Count - 1));
    }


    // 결합
    public bool Merge()
    {
        if (!IsMerge) return false;
        //결합 알리기
        Presenter.Send(DEF.Game, "Merge", this);
        return true;
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
        if (index == 0)
        {
            pos.x += 0.16f;
            pos.y += 0.19f;
            return pos;
        }
        else if (index == 1)
        {
            pos.x -= 0.16f;
            pos.y += 0.19f;
            return pos;
        }
        else
        {
            pos.y += 0.07f;
            return pos;
        }


    }

    public void Refresh()
    {
        if (UnitCount == 0) return;

        for (int i = 0; i < _unitList.Count; ++i)
            _unitList[i].SetMovePosition(GetUnitPosition(i));
    }




}
