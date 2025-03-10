using System.Collections.Generic;
using System.Linq;
using GB;
using UnityEngine;

public class Board : MonoBehaviour
{
    const float TILE_SZ = 1;
    const int TILE_WIDTH = 6;
    const int TILE_HEIGHT = 3;

    Tile[] _myTiles;
    Tile[] _friendTiles;
    WaveMacine _waveMacine;
    

    public void Init()
    {
        //타일 생성 및 타일 초기화
        _myTiles = new Tile[TILE_WIDTH * TILE_HEIGHT];
        Vector2 startPos = new Vector2(-2.5f,-1.5f);
        for (int i = 0; i < _myTiles.Length; ++i) 
        {
            _myTiles[i] = new Tile();
            float w = i % TILE_WIDTH;
            float h =  i / TILE_WIDTH;
            _myTiles[i].Position = new Vector2(startPos.x + w, startPos.y - h);
        }

        _friendTiles = new Tile[TILE_WIDTH * TILE_HEIGHT];
        startPos = new Vector2(-2.5f,2.5f);
        for (int i = 0; i < _friendTiles.Length; ++i) 
        {
            _friendTiles[i] = new Tile();
            float w = i % TILE_WIDTH;
            float h =  i / TILE_WIDTH;
            _friendTiles[i].Position = new Vector2(startPos.x + w, startPos.y - h);
        }

        _waveMacine = new WaveMacine();
        _waveMacine.Init(this);
    }
    /// <summary>
    /// 총 유닛 수
    /// </summary>
    /// <returns>유닛 수</returns>
    public int GetUnitCount(int playerID)
    {
        int count = 0;

        if(playerID == 0)
        {
            for(int i = 0; i< _myTiles.Length; ++i) count += _myTiles[i].UnitCount;
        }
        else
        {
            for(int i = 0; i< _friendTiles.Length; ++i) count += _friendTiles[i].UnitCount;
        }

        return count;
    }

    public Tile[] GetTiles(int playerid)
    {
        if(playerid == 0) return _myTiles;
        else              return _friendTiles;
    }



    public Tile GetTile(int index, int playerid)
    {
        if(playerid == 0) return _myTiles[index];
        else return _friendTiles[index];
    }

    public Tile GetTile(Vector2 point)
    {
        int idx = GetTouchTileIndex(point);
        if(idx == -1) return null;
        return _myTiles[idx];
    }

    public int GetTileLength()
    {
        return _myTiles.Length;
    }

    public void SwapTileUnits(Tile A_tile,Tile B_tile)
    {
        if(A_tile == null || B_tile == null) return;
        A_tile.SwapUnits(B_tile);
    }
    
    /// <summary>
    /// 유닛이 타일의 유닛들에 꼽사리 낄 자리 점검
    /// </summary>
    /// <param name="unitID"></param>
    /// <returns></returns> <summary>
    public int ContainsUnitTeam(string unitID , int playerid )
    {
        for(int i = 0; i< _myTiles.Length; ++i)
        {
            if(playerid == 0)
            {
                //유닛 아이디가 같으며, 유닛 무게가 Max가 아닐 경우 
                if(_myTiles[i].UnitID == unitID && !_myTiles[i].Max) return i;
            }
            else
            {
                if(_friendTiles[i].UnitID == unitID && !_friendTiles[i].Max) return i;
            }
            
        }
        return -1;
    }

    /// <summary>
    /// 유닛 추가
    /// </summary>
    /// <param name="unitID">유닛ID</param>
    public Tile AddUnit(Player player,Unit unit)
    {
        Tile[] tiles = player.ID == 0 ? _myTiles : _friendTiles;
        int idx = ContainsUnitTeam(unit.ID,player.ID);
        //낄자리 없음
        if(idx == -1)
        {
            unit.transform.SetParent(transform);
            //타일중 빈자리 검색
            var list = tiles.Where(v=> v.UnitID == null).ToList();
            //Random 자리 
            int rand = Random.Range(0,list.Count);
            list[rand].AddUnit(unit);
            CreateBoingFX().transform.position = list[rand].Position;
            CreateCreateFX().transform.position = list[rand].Position;
            return list[rand];
        }
        else
        {
            unit.transform.SetParent(transform);
            tiles[idx].AddUnit(unit);
            CreateBoingFX().transform.position = tiles[idx].Position;
            CreateCreateFX().transform.position = tiles[idx].Position;
            return tiles[idx];
        }
    }

    /// <summary>
    /// 생성 파티클 
    /// </summary>
    /// <returns>파티클</returns>
    GameObject CreateBoingFX()
    {
        var eff = ObjectPooling.Create(RES_PREFAB.FX_BOING,5);
        eff.transform.SetParent(transform);
        
        return eff;
    }
    GameObject CreateCreateFX()
    {
         var eff = ObjectPooling.Create(RES_PREFAB.FX_UnitCreateFX,5);
        eff.transform.SetParent(transform);

        return eff;
    }

    
    
    

    /// <summary>
    /// 터치 위치에 따른 타일 인덱스
    /// </summary>
    /// <param name="pos">터치 포인트</param>
    /// <returns>타일 인덱스</returns>
    public int GetTouchTileIndex(Vector2 point)
    {
        //타일맵의 Half X
        float mapHW = (float)TILE_WIDTH / 2;

        //타일 벗어난 우 터치
        if (point.x > mapHW) return -1;
        //타일 벗어난 좌 터치
        if (point.x < -mapHW) return -1;
        //타일 벗어난 상단 터치
        if (point.y > -1) return -1;
        //타일 벗어난 하단 터치
        if (point.y < -1 - TILE_HEIGHT) return -1;

        int tx = (int)(point.x + mapHW);
        int ty = Mathf.Abs((int)(point.y + 1));

        return ty * TILE_WIDTH + tx;
    }

    public void WaveStart(int wave)
    {
        _waveMacine.WaveStart(wave,()=>{Presenter.Send(DEF.Game,DEF.P_WAVE_END);});
        
        
    }
    public bool IsBossWave()
    {
        return _waveMacine.IsBossWave;
    }



    void Update()
    {
        _waveMacine.Update(GBTime.GetDeltaTime(DEF.T_GAME));
    }

    List<GameObject> _mobList = new List<GameObject>();

    public void AddMob(GameObject mob)
    {
        mob.transform.SetParent(transform);
        _mobList.Add(mob);

        //몬스터가 MAXCOUNT 보다 많은 경우 게임오버
        if (_mobList.Count >= DEF.MOB_MAXCOUNT)
        {
            _waveMacine.Stop();
            Presenter.Send(DEF.Game, DEF.GameOver);
        }

        Presenter.Send(DEF.P_GameScene, "WaveCount", _mobList.Count);
    }
    public void RemoveMob(GameObject mob)
    {
        if(_waveMacine.IsBossWave)
        {
            if(mob.GetComponent<Mob>().MobType  == "B") _waveMacine.BossKill();            
        }

        
        _mobList.Remove(mob);

        Presenter.Send(DEF.P_GameScene, "WaveCount", _mobList.Count);
    }

}
