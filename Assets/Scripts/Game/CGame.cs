
using GB;
using UnityEngine;

public class CGame : MonoBehaviour, IView
{
    [SerializeField] Board _board;

    int _wave = 1;

    [SerializeField] GameObject _touchBeganCircle;
    [SerializeField] GameObject _touchMovedCircle;

    //점선
    [SerializeField] GuideLine _line;

    [SerializeField] GameObject _worldButtons;
    
    //합성 버튼
    [SerializeField] GameObject _mergeButton;


    //터치시 타일
    Tile _beganTile;
    Tile _targetTile;


    void Awake()
    {
        _board.Init();
        Presenter.Bind(DEF.Game, this);
        ODataBaseManager.Set(DEF.Game, this);
        InputController.I.TouchWorldEvent += OnTouch;
        _touchBeganCircle.SetActive(false);
        _touchMovedCircle.SetActive(false);
         ActiveWorldButton(false,Vector2.zero);
    }

    void OnDisable()
    {
        Presenter.UnBind(DEF.Game, this);
        ODataBaseManager.Remove(DEF.Game);
        InputController.I.TouchWorldEvent -= OnTouch;
    }
    void Start()
    {
        _wave = 1;
        // _board.WaveStart(_wave);
    }


    /// <summary>
    /// 게임 시작
    /// </summary>
    public void GameStart()
    {
        // _board.GameStart();
    }

    /// <summary>
    /// 게임오버
    /// </summary>
    public void GameOver()
    {
        // _board.GameStop();
    }

    /// <summary>
    /// 소환
    /// </summary>
    public void Summon()
    {
        GBLog.Log("SummonButton");

        //유닛수 체크

        //재화 체크

        //유닛 생성
        _board.AddUnit("Mushroom");
        ActiveWorldButton(false,Vector2.zero);

    }

    /// <summary>
    /// 도박
    /// </summary>
    public void Gacha()
    {

    }

    /// <summary>
    /// 신화
    /// </summary>
    public void Myth()
    {

    }

    /// <summary>
    /// 강화
    /// </summary>
    public void Upgrade()
    {

    }

    public void Merge()
    {
        if(_targetTile == null)  return;
        Debug.Log(_targetTile.UnitID);

    }

    public void SellUnit()
    {
        
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
        if(!_worldButtons.activeSelf) _targetTile = tile;
        
        switch (phase)
        {
            case TouchPhase.Began:
                if (tile != null && tile.UnitCount > 0)
                {
                    _touchBeganCircle.transform.position = tile.Position;
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
                    _touchMovedCircle.SetActive(false);
                    _touchBeganCircle.SetActive(false);
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
        _worldButtons.SetActive(isActive);
        if(isActive)
        {
            _mergeButton.SetActive(_targetTile.UnitMax);
            _worldButtons.transform.position = position;     
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
                    _touchBeganCircle.SetActive(true);
                    _touchBeganCircle.transform.position = tile.Position;
                }

                break;

            case TouchPhase.Moved:
                if (tile != null && _beganTile != null)
                {
                    _touchMovedCircle.SetActive(true);
                    _touchMovedCircle.transform.position = tile.Position;

                    _line.gameObject.SetActive(true);
                    _line.SetPoint(_beganTile.Position, tile.Position);

                }
                break;

            case TouchPhase.Ended:
                if (tile != null && _beganTile != null)
                {
                    _touchBeganCircle.SetActive(true);
                    _touchBeganCircle.transform.position = tile.Position;
                }
                else
                {
                    _touchBeganCircle.SetActive(false);
                }

                _line.gameObject.SetActive(false);
                _touchMovedCircle.SetActive(false);

                break;

        }

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
                    _wave++;
                    _board.WaveStart(_wave);
                    GBLog.Log("GAME", "WaveStart" + _wave, Color.green);
                });
                break;

            case DEF.DEAD_MOB:
                _board.RemoveMob(data.Get<GameObject>());
                break;


        }

    }
}
