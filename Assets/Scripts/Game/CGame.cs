
using GB;
using UnityEngine;

public class CGame : MonoBehaviour,IView
{
    [SerializeField] Board _board;

    int _wave = 1;


    void Awake()
    {
        _board.Init();
        Presenter.Bind(DEF.Game,this);
        ODataBaseManager.Set(DEF.Game,this);
        InputController.I.TouchWorldEvent += OnTouch;
    }

    void OnDisable()
    {
        Presenter.UnBind(DEF.Game,this);
        ODataBaseManager.Remove(DEF.Game);
        InputController.I.TouchWorldEvent -= OnTouch;
    }
    void Start()
    {
        _wave = 1;
        _board.WaveStart(_wave);
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
        
        switch(phase)
        {
            case TouchPhase.Began:
            
            break;

            case TouchPhase.Moved:
            break;

            case TouchPhase.Ended:
            break;

            case TouchPhase.Canceled:
            break;
        }

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

    public void ViewQuick(string key, IOData data)
    {
        switch(key)
        {
            case DEF.GameOver:
            GBLog.Log("GAME","GAMEOVER",Color.red);
            GameOver();
            break;
            case DEF.P_WAVE_END:
            Timer.Create(1,()=>
            {
                _wave ++;
                _board.WaveStart(_wave);
                GBLog.Log("GAME","WaveStart" +_wave,Color.green);
            });
            break;

            case DEF.DEAD_MOB:
            _board.RemoveMob(data.Get<GameObject>());
            break;


        }
        
    }
}
