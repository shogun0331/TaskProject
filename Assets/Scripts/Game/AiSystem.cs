using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class AiSystem : MonoBehaviour
{
    Player _player;
    enum STATE { Summon = 0, Gacha, Upgrade, MYTH, UNIT_MOVE, Merge };
    float time = 0;
    const float AI_TIMER = 3;
    bool _isPlaying;


    public void Init(Player player)
    {
        _player = player;
    }


    public void Play()
    {
        
        StartCoroutine(Respone());
    }
    IEnumerator Respone()
    {

        yield return new WaitForSeconds(1f);

        for(int i = 0; i< 5; ++i)
        {
            yield return new WaitForSeconds(0.5f);
            TileMove( _player.Summon());
            
        }
        _isPlaying = true;

    }

    public void Stop()
    {
        _isPlaying = false;
    }
    void Update()
    {
        if (!_isPlaying) return;
        time += GBTime.GetDeltaTime(DEF.T_GAME);
        if (time > AI_TIMER)
        {
            time = 0;
            Move();
        }
    }

    STATE RandState()
    {
        int rand = Random.Range(0,100);
        if(rand <= 30) return STATE.Summon;
        else if(rand <= 50) return STATE.UNIT_MOVE;
        else if(rand <= 70) return STATE.Merge;
        else if(rand <= 80) return STATE.Gacha;
        else if(rand <= 90) return STATE.Upgrade;
        else  return STATE.MYTH;
    }

    void Move()
    {
        STATE state = RandState();

        switch (state)
        {
            case STATE.Summon:
                TileMove( _player.Summon());
                break;
            case STATE.Gacha:
                Gacha();
                break;
            case STATE.Upgrade:
                Upgrade();
                break;
            case STATE.MYTH:
                MYTH();
                break;
            case STATE.UNIT_MOVE:
                UNIT_MOVE();
                break;
            case STATE.Merge:
                Merge();
                break;
        }

    }

    void Merge()
    {
        var tiles = _player.GetTiles();
        for (int i = 0; i < tiles.Length; ++i)
        {
            if (tiles[i].Max)
            {
                tiles[i].Merge();
                break;
            }
        }
    }

    void TileMove(Tile tile)
    {
        if(tile == null) return;
        var tiles = _player.GetTiles();
        for (int i = 0; i < tiles.Length; ++i)
        {
            if (tiles[i].UnitCount > 0)
            {
                if(tiles[0].UnitCount == 0) tile.SwapUnits(tiles[0]);
                else if(tiles[6].UnitCount == 0) tile.SwapUnits(tiles[6]);
                else if(tiles[12].UnitCount == 0) tile.SwapUnits(tiles[12]);
                else if(tiles[13].UnitCount == 0) tile.SwapUnits(tiles[13]);
                else if(tiles[7].UnitCount == 0) tile.SwapUnits(tiles[7]);
                else if(tiles[14].UnitCount == 0) tile.SwapUnits(tiles[14]);
                else if(tiles[15].UnitCount == 0) tile.SwapUnits(tiles[15]);
                else if(tiles[8].UnitCount == 0) tile.SwapUnits(tiles[8]);
                else if(tiles[9].UnitCount == 0) tile.SwapUnits(tiles[9]);
                else tile.SwapUnits(tiles[Random.Range(0,tiles.Length)]);

                
                break;
            }
        }
    }

    void UNIT_MOVE()
    {
        var tiles = _player.GetTiles();
        for (int i = 0; i < tiles.Length; ++i)
        {
            if (tiles[i].UnitCount > 0)
            {
                
                if(tiles[0].UnitCount == 0) tiles[i].SwapUnits(tiles[0]);
                else if(tiles[6].UnitCount == 0) tiles[i].SwapUnits(tiles[6]);
                else if(tiles[12].UnitCount == 0) tiles[i].SwapUnits(tiles[12]);
                else if(tiles[13].UnitCount == 0) tiles[i].SwapUnits(tiles[13]);
                else if(tiles[7].UnitCount == 0) tiles[i].SwapUnits(tiles[7]);
                else if(tiles[14].UnitCount == 0) tiles[i].SwapUnits(tiles[14]);
                else if(tiles[15].UnitCount == 0) tiles[i].SwapUnits(tiles[15]);
                else if(tiles[8].UnitCount == 0) tiles[i].SwapUnits(tiles[8]);
                else if(tiles[9].UnitCount == 0) tiles[i].SwapUnits(tiles[9]);
                else tiles[i].SwapUnits(tiles[Random.Range(0,tiles.Length)]);

                
                break;
            }
        }
    }

    void MYTH()
    {
        var table = GameDataManager.GetTable<MythTable>();

        for (int i = 0; i < table.Count; ++i)
        {
            TileMove( _player.SummonMythMerge(table[i].ID));
        }
            
    }

    void Upgrade()
    {
        int rand = Random.Range(0,100);

        string id = null;

        if(rand <= 20)id = "C";
        else if(rand <=40) id = "A";
        else if(rand <= 60) id = "S";
        else  id = "Summon";

        if(_player.Levels[id] >= 12) return;
        if(!CheckCurrency(id)) return;
        _player.Upgrade(id);


    }

     public bool CheckCurrency(string id)
    {
        string cType = CurrencyType(id);
        if(cType == null) return false;

        if(cType == "GOLD")
        {
             if(_player.Gold >= GetPrice(id))
             return true;
        }

        else if(cType == "LUCK")
        {
             if(_player.Luck >= GetPrice(id))
             return true;
        }

        return false;

    }

    
    public string CurrencyType(string id)
    {
        var table = GameDataManager.GetTable<UpgradeTable>();
        var list = table.Datas.Where(v => v.UpgradeType == id).ToList();
        if(list == null) return null;;
        var prob = list.FirstOrDefault(v => v.Level == _player.Levels[id] + 1);
        if(prob == null) return null;
        return prob.PrieceID;

    }

    int GetPrice(string id)
    {
        var table = GameDataManager.GetTable<UpgradeTable>();
        var list = table.Datas.Where(v => v.UpgradeType == id).ToList();
        if(list == null) return -1;
        var prob = list.FirstOrDefault(v => v.Level == _player.Levels[id] + 1);
        if(prob == null) return -1;
        return prob.PriceValue;
    }


    void Gacha()
    {
        var table = GameDataManager.GetTable<GachaTable>();

        int rand = Random.Range(0, 100);
        if (rand <= 50)
        {
            if (_player.Luck >= table["A"].PriceValue)
            {
                _player.SubtracktLUCK(table["A"].PriceValue);
                int r = Random.Range(0, 100);
                if (r <= table["A"].Weight) _player.GachaUnit("A");
            }

        }
        else if (rand <= 90)
        {
            if (_player.Luck >= table["B"].PriceValue)
            {
                _player.SubtracktLUCK(table["B"].PriceValue);
                int r = Random.Range(0, 100);
                if (r <= table["B"].Weight) _player.GachaUnit("B");
            }
        }
        else
        {
            if (_player.Luck >= table["C"].PriceValue)
            {
                _player.SubtracktLUCK(table["C"].PriceValue);
                int r = Random.Range(0, 100);
                if (r <= table["C"].Weight) _player.GachaUnit("C");
            }
        }

    }










}
