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



    public void Init()
    {
        _myTiles = new Tile[TILE_WIDTH * TILE_HEIGHT];
        for(int i = 0; i<_myTiles.Length; ++i )_myTiles[i] = new Tile();

        _friendTiles = new Tile[TILE_WIDTH * TILE_HEIGHT];
        for(int i = 0; i<_friendTiles.Length; ++i )_friendTiles[i] = new Tile();

    }


    /// <summary>
    /// 터치 위치에 따른 타일 인덱스
    /// </summary>
    /// <param name="pos">터치 포인트</param>
    /// <returns>타일 인덱스</returns>
    public int GetTouchTileIndex(Vector2 point)
    {
        //타일맵의 Half X
        float mapHW = (float)TILE_WIDTH/2;
        
        //타일 벗어난 우 터치
        if(point.x > mapHW ) return -1;
        //타일 벗어난 좌 터치
        if(point.x < -mapHW) return -1;
        //타일 벗어난 상단 터치
        if(point.y > -1 ) return -1;
        //타일 벗어난 하단 터치
        if(point.y < -1 - TILE_HEIGHT) return -1;

        int tx = (int)(point.x + mapHW);
        int ty = Mathf.Abs((int)(point.y + 1));

        return ty * TILE_WIDTH + tx;
    }

    public void GameStart()
    {

    }

    public void GameStop()
    {
        
    }
  
}
