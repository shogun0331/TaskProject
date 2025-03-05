using GB;
using UnityEngine;

public class CGame : MonoBehaviour
{
    [SerializeField] Board _board;

    bool _isGameOver;

    //게임오버 구독 알림
    public bool isGameOver
    {
        get{return _isGameOver;}
        set
        {
            _isGameOver = value;
            ODataBaseManager.Set(DEF.GameOver,_isGameOver);
        }
    }

    void Awake()
    {
        _board.Init();
        ODataBaseManager.Set(DEF.Game,this);
        isGameOver = false;
        InputController.I.TouchWorldEvent += OnTouch;
    }

    void OnDisable()
    {
        ODataBaseManager.Remove(DEF.Game);
        InputController.I.TouchWorldEvent -= OnTouch;
    }


    /// <summary>
    /// 터치 및 클릭 콜백 
    /// </summary>
    /// <param name="phase">터치 상태</param>
    /// <param name="touchID">터치 ID</param>
    /// <param name="position">터치 월드 포지션</param>
    void OnTouch(TouchPhase phase, int touchID, Vector2 position)
    {
        
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
        _board.GameStart();
    }


    /// <summary>
    /// 게임오버
    /// </summary>
    public void GameOver()
    {
        isGameOver = true;
        _board.GameStop();
    }


    /// <summary>
    /// 소환
    /// </summary>
    public void Summon()
    {

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

}
