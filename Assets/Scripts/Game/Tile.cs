using System.Collections.Generic;
using GB;
using UnityEngine;

public class Tile
{
    public string ID;
    public Vector2 Position;
    public List<Unit> UnitList = new List<Unit>();


    // 결합이 가능한 상태 인가?
    public bool IsMerge
    {
        get
        {
            if (UnitList == null || UnitList.Count < 3) return false;
            return true;
        }
    }

    public int UnitCount
    {
        get
        {
            if (UnitList == null) return 0;
            return UnitList.Count;
        }
    }

    public bool UnitMax
    {
        get
        {
            if (UnitList == null) return false;
            return UnitList.Count == 3;
        }
    }
    public string UnitID
    {
        get
        {
            if (UnitList == null || UnitList.Count == 0 || UnitList[0] == null) return null;
            return UnitList[0].ID;
        }
    }

    public void AddUnit(Unit unit)
    {
        UnitList.Add(unit);
        unit.transform.position = Position;
        unit.SetMovePosition(GetUnitPosition(UnitList.Count - 1));
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
        for (int i = 0; i < UnitList.Count; ++i) ObjectPooling.Return(UnitList[i].gameObject);
        UnitList.Clear();
    }

    public void 
    SwapUnits(Tile tile)
    {
        var list = tile.UnitList;
        tile.UnitList = UnitList;
        UnitList = list;
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

        for (int i = 0; i < UnitList.Count; ++i)
            UnitList[i].SetMovePosition(GetUnitPosition(i));
    }




}
