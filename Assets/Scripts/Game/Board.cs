using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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

    public Tile GetTile(Vector2 point)
    {
        int idx = GetTouchTileIndex(point);
        if(idx == -1) return null;
        return _myTiles[idx];
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
        if (_mobList.Count > DEF.MOB_MAXCOUNT)
        {
            _waveMacine.Stop();
            Presenter.Send(DEF.Game, DEF.GameOver);
        }

        Presenter.Send(DEF.P_GameScene, "WaveCount", _mobList.Count);
    }
    public void RemoveMob(GameObject mob)
    {
        _mobList.Remove(mob);

        Presenter.Send(DEF.P_GameScene, "WaveCount", _mobList.Count);
    }

}
