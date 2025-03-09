using System;

using GB;
using UnityEngine;

[Serializable]
public class WaveMacine
{
    MobTable _mobTable;
    NomalWaveTable _nomalTable;

    float _waveTime;
    float _waveCreateTime;
    float _waveDelay = 0.5f;

    string _waveID;
    int _waveCount = 0;
    float _waveTotalTime;
    bool _isBossWave;

    public bool IsBossWave{get{return _isBossWave;}}
    bool _isWavePlaying;
    Action _result;
    int _MobCount;

    int _bossKillCnt = 0;


    Vector2[] BOT_PATH = new Vector2[]
    {
        new Vector2(-3.5f,-4.5f),
        new Vector2(-3.5f,-0.5f),
        new Vector2(3.5f,-0.5f),
        new Vector2(3.5f,-4.5f)
    };

    Vector2[] TOP_PATH = new Vector2[]
    {
        new Vector2(-3.5f, 3.5f),
        new Vector2(-3.5f, -0.5f),
        new Vector2(3.5f, -0.5f),
        new Vector2(3.5f, 3.5f),
    };

    Board _board;

    public void Init(Board board)
    {
        if (_mobTable == null) _mobTable = GameDataManager.GetTable<MobTable>();
        if (_nomalTable == null) _nomalTable = GameDataManager.GetTable<NomalWaveTable>();

        _board = board;
    }

    public void WaveStart(int wave, Action result)
    {
        _checkTime = 0;
        _result = result;
        _waveCount = 0;
        _waveTime = 0;
        _waveCreateTime = 0;
        _waveID = wave.ToString();
        if (!_nomalTable.ContainsKey(_waveID)) 
        {
            Stop();
            Presenter.Send(DEF.Game, DEF.GameOver);
            return;
        }

        if (string.IsNullOrEmpty(_nomalTable[_waveID].BossID))
        {
            _isBossWave = false;
            _waveDelay = _nomalTable[_waveID].Dealay;
            _waveTotalTime = _nomalTable[_waveID].CreateCount * _waveDelay;
        }
        else
        {
            _isBossWave = true;
            _waveTotalTime = _nomalTable[_waveID].Dealay;
            _bossKillCnt = 0;
        }

        CreateMob();

        if(_isBossWave)
            Timer.Create(0.5f,CreateMob);
        
        Presenter.Send(DEF.P_GameScene, "Time", (int)(_waveTotalTime - _waveTime));
        Presenter.Send(DEF.P_GameScene,"Wave",wave);

        _isWavePlaying = true;
    }
    public void Stop()
    {
        _isWavePlaying = false;
    }

    int _checkTime;

    

    void CreateMob()
    {
        
        string mobName = _mobTable[_nomalTable[_waveID].MobID].Name;
        var mob = ObjectPooling.Create("Mobs/" + mobName).GetComponent<Mob>();
        if(_waveCount % 2  == 0 ) mob.MobSetting(_nomalTable[_waveID].MobID).SetMovePath(BOT_PATH).Play();
        else mob.MobSetting(_nomalTable[_waveID].MobID).SetMovePath(TOP_PATH).Play();
        _board.AddMob(mob.gameObject);

        _waveCount ++;
        
    }

    public void BossKill()
    {
        _bossKillCnt ++;
          
    }

    public void Update(float dt)
    {
        if (!_isWavePlaying) return;

        _waveTime += dt;

        int time = (int)_waveTime;
        if (time > _checkTime)
        {
            _checkTime = time;
            Presenter.Send(DEF.P_GameScene, "Time", (int)(_waveTotalTime - _waveTime));
        }

        if (!_isBossWave)
        {
            _waveCreateTime += dt;

            if (_waveCreateTime > _waveDelay)
            {
                _waveCreateTime = 0;
                CreateMob();
            }

            //웨이브 를 모두 생성 완료 한 경우 웨이브 완료
            if (_waveCount >= _nomalTable[_waveID].CreateCount)
            {
                Stop();
                _result?.Invoke();
            }
        }
        else
        {
            if(_bossKillCnt == 2)
            {
                Stop();
                _result?.Invoke();
                Presenter.Send(DEF.Game, DEF.BOSS_CLEAR);
            }

            if (_waveTime > _waveTotalTime)
            {
                Stop();
                Presenter.Send(DEF.Game, DEF.GameOver);
            }

        }


    }
}
