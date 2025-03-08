
using System.Collections.Generic;
using GB;
using QuickEye.Utility;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class CGame : MonoBehaviour, IView
{
    [SerializeField] Board _board;
    [SerializeField] GuideLine _line;
    [SerializeField] UnityDictionary<string,GameObject> _dictGameObjects = new UnityDictionary<string, GameObject>();
    
    int _wave = 1;
    
    //터치시 타일
    Tile _beganTile;
    Tile _targetTile;

    //플레이어
    Player _myPlayer;
    Player _friendPlayer;
    Text _sellText;


    void Awake()
    {
        Init();
        _board.Init();
        Presenter.Bind(DEF.Game, this);
        ODataBaseManager.Set(DEF.Game, this);
        InputController.I.TouchWorldEvent += OnTouch;
        Application.targetFrameRate = 60;
        _sellText = _dictGameObjects["SellText"].GetComponent<Text>();
    }

    void OnDisable()
    {
        Presenter.UnBind(DEF.Game, this);
        ODataBaseManager.Remove(DEF.Game);
        InputController.I.TouchWorldEvent -= OnTouch;
    }

    void Start()
    {
        GameStart();
     
    }


    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        _wave = 1;
        _board.WaveStart(_wave);
    }

    /// <summary>
    /// 다음 웨이브 시작
    /// </summary>
    public void NextWave()
    {
        _wave ++;
        _board.WaveStart(_wave);
    }

    /// <summary>
    /// 게임오버
    /// </summary>
    public void GameOver()
    {
        GBTime.Stop(DEF.T_GAME);
        _ai.Stop();
    }

    /// <summary>
    /// 소환
    /// </summary>
    public void Summon()
    {
        _myPlayer.Summon();
        ActiveWorldButton(false,Vector2.zero);
    }

    public void SellUnit()
    {
        _targetTile.SellUnit();
        if(_targetTile.UnitCount == 0) ActiveWorldButton(false,Vector2.zero); 
        ActiveWorldButton(false,Vector2.zero);
    }

    public void Merge()
    {
        _targetTile.Merge();
        ActiveWorldButton(false,Vector2.zero);
    }

    


    /// <summary>
    /// 터치 및 클릭 콜백 
    /// </summary>
    /// <param name="phase">터치 상태</param>
    /// <param name="touchID">터치 ID</param>
    /// <param name="position">터치 월드 포지션</param>
    void OnTouch(TouchPhase phase, int touchID, Vector2 position)
    {

        var tile = _board.GetTile(position);
        if(!_dictGameObjects["WorldButtons"].activeSelf) _targetTile = tile;
        
        switch (phase)
        {
            case TouchPhase.Began:
                if (tile != null && tile.UnitCount > 0)
                {
                    _dictGameObjects["TouchBeganCircle"].transform.position = tile.Position;
                    _beganTile = tile;
                }
                else
                {
                    _beganTile = null;
                }

                
           
                
                GetTouchMoveCircle(phase, tile);
                break;

            case TouchPhase.Moved:
                GetTouchMoveCircle(phase, tile);
                ActiveWorldButton(false,Vector2.zero);
                break;

            case TouchPhase.Ended:
                
                if(_beganTile != null && tile == _beganTile)
                {
                    //Show 합성 버튼 업그레이드 버튼
                    ActiveWorldButton(true,_beganTile.Position);
                    _beganTile = null;
                }
                else if (tile != null && _beganTile != null)
                {
                    tile.SwapUnits(_beganTile);
                    _beganTile = null;
                    ActiveWorldButton(false,Vector2.zero);
                }
                else
                {
                    ActiveWorldButton(false,Vector2.zero);
                }
                
                GetTouchMoveCircle(phase, tile);
                
                break;

            case TouchPhase.Canceled:
                if(_beganTile != tile)
                {
                    _beganTile = null;
                    _line.gameObject.SetActive(false);
                    
                    _dictGameObjects["TouchMovedCircle"].SetActive(false);
                    _dictGameObjects["TouchBeganCircle"].SetActive(false);
                }
                break;
        }

    }

    /// <summary>
    /// 합성 업그레이드 버튼
    /// </summary>
    /// <param name="isActive">온오프</param>
    /// <param name="position">위치</param>
    void ActiveWorldButton(bool isActive , Vector2 position)
    {
        _dictGameObjects["WorldButtons"].SetActive(isActive);
        if(isActive)
        {
            _sellText.text = string.Format("판매 "+_targetTile.Price) ;
            _dictGameObjects["Button_Merge"].SetActive(_targetTile.Max && (int)_targetTile.Rank < (int)UnitRank.A);
            _dictGameObjects["WorldButtons"].transform.position = position;     
        }
    }


/// <summary>
///  터치 이동 가이드
/// </summary>
/// <param name="phase">터치타입</param>
/// <param name="tile">타일</param> <summary>
    void GetTouchMoveCircle(TouchPhase phase, Tile tile)
    {
        switch (phase)
        {

            case TouchPhase.Began:
                if (tile != null && _beganTile != null)
                {
                    _dictGameObjects["TouchBeganCircle"].SetActive(true);
                    _dictGameObjects["TouchBeganCircle"].transform.position = tile.Position;
                    _dictGameObjects["TouchDistance"].SetActive(true);
                    _dictGameObjects["TouchDistance"].transform.position = tile.Position;
                    _dictGameObjects["TouchDistance"].transform.localScale = new Vector3( tile.AttackDist,tile.AttackDist,tile.AttackDist);
                }

                break;

            case TouchPhase.Moved:
                if (tile != null && _beganTile != null)
                {
                    _dictGameObjects["TouchMovedCircle"].SetActive(true);
                    _dictGameObjects["TouchMovedCircle"].transform.position = tile.Position;
                    _line.gameObject.SetActive(true);
                    _line.SetPoint(_beganTile.Position, tile.Position);
                }
                
                break;

            case TouchPhase.Ended:
                if (tile != null && _beganTile != null)
                {
                    _dictGameObjects["TouchBeganCircle"].SetActive(true);
                    _dictGameObjects["TouchBeganCircle"].transform.position = tile.Position;
                }
                else
                {
                    _dictGameObjects["TouchBeganCircle"].SetActive(false);
                }

                _line.gameObject.SetActive(false);
                _dictGameObjects["TouchMovedCircle"].SetActive(false);
                if(_dictGameObjects["TouchDistance"].activeSelf)_dictGameObjects["TouchDistance"].SetActive(false);
                

                break;
        }

    }


    AiSystem _ai;
    

    void Init()
    {
         var table = GameDataManager.GetTable<UnitTable>();
        _myPlayer = new Player();
        var unitDataList =  new List<UnitData>();

        //실제 데이터는 유저 데이터를 보고 입력 할 것
        for(int i =0; i<table.Count;++i)
        {
            var u = unitDataList.FirstOrDefault(v=> v.ID == table[i].UnitID);
            if(u == null)
            {
                unitDataList.Add( new UnitData
                {
                    Rank = table[i].Rank,
                    ID = table[i].UnitID,
                    Level = table[i].Level
                });
            }
        }

        _myPlayer.Init(_board,unitDataList,0);
    
        _friendPlayer = new Player();
        //실제 데이터는 AI 저장 데이터를 보고 입력 할 것
        _friendPlayer.Init(_board,unitDataList,1);
        _ai = gameObject.AddComponent<AiSystem>();
        _ai.Init(_friendPlayer);
        _ai.Play();


  
        _dictGameObjects["TouchBeganCircle"].SetActive(false);
        _dictGameObjects["TouchMovedCircle"].SetActive(false);
         ActiveWorldButton(false,Vector2.zero);

    }


    public void ViewQuick(string key, IOData data)
    {
        switch (key)
        {
            case DEF.GameOver:
                GBLog.Log("GAME", "GAMEOVER", Color.red);
                GameOver();
                break;
            case DEF.P_WAVE_END:
                Timer.Create(1, () =>
                {
                    NextWave();
                    GBLog.Log("GAME", "WaveStart" + _wave, Color.green);
                });
                break;

            case DEF.DEAD_MOB:
                var mob = data.Get<Mob>();
                _myPlayer.DeadMob(mob.Data);
                _friendPlayer.DeadMob(mob.Data);
                _board.RemoveMob(data.Get<Mob>().gameObject);
                break;

         

        }

    }
}
