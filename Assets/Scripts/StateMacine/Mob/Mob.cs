
using GB;
using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public enum MobState{Idle,Move,Dead}
public class Mob : StateMachine<MobState>,IBody
{
    //생성 고유 ID
    int _index;
    public int Index {get{return _index;}}

    //MobTable 고유 ID
    [SerializeField] string _id;
    public string ID{get{return _id;}}
    
    // 능력치
    Status _status;

    public Status status{get{return _status;}}


    //현재 체력
    int _hp;
    // 테이블 데이터
    MobTableProb _data;
    
    // 이동 경로
    Vector2[] _movePaths;
    public Vector2[] MovePaths{get{return _movePaths;}}
    
    // 이동 경로 인덱스
    public int moveIdx = 0;

    // 스프라이트 애니메이션
    [SerializeField] SPRAnimation _anim;
    [SerializeField] SpriteRenderer _sRender;

    //이동 방해 0 ~ 1 Max
    private float _moveSlow = 0;
    public float MoveSlow{get{return _moveSlow;}}

    [SerializeField] string _hpViewPath;

    [HideInInspector] public HpView hpView;

    void Awake()
    {
        Init();
    }

    // void Start()
    // {
    //     MobSetting("0");
    //     List<Vector2> list = new List<Vector2>();
    //     list.Add(new Vector2(-3.5f,-4.5f));
    //     list.Add(new Vector2(-3.5f,-0.5f));
    //     list.Add(new Vector2(3.5f,-0.5f));
    //     list.Add(new Vector2(3.5f,-4.5f));

    //     SetMovePath(list.ToArray()).Play();
    // }

    public void SetFlipX(bool isFlip)
    {
        _sRender.flipX = isFlip;
    }

    public void SetAnimation(string animName)
    {
        _anim.Play(animName);
    }

    public Mob MobSetting(string id)
    {
        if(_data == null)
        {
            var table =GameDataManager.GetTable<MobTable>();
            if(table.ContainsKey(id))
            {
                _data = table[id];
            }
            else
            {
                Debug.LogError("Mob ID Error!! "+ id);
                return this;
            }
        }

        _status = new Status()
        {
            MAX_HP = _data.MAX_HP,
            M_SPD = _data.M_SPD,
            DEF = _data.DEF
        };

        _hp = _status.MAX_HP;


        //HP 월드캔버스에 UI 생성
        hpView = ObjectPooling.Create(_hpViewPath,100).GetComponent<HpView>();

        //WorldCanvas 부모로 이동
        if(ODataBaseManager.Contains("WorldCanvas"))
        {
            hpView.transform.SetParent(ODataBaseManager.Get<Transform>("WorldCanvas"));
            hpView.transform.localScale = Vector3.one;
            
        }
        else
        {
            ODataBaseManager.Bind(this,"WorldCanvas",(value)=>
            {
                hpView.transform.SetParent(value.Get<Transform>());
                ODataBaseManager.UnBind(this,"WorldCanvas");
                hpView.transform.localScale = Vector3.one;
            });
        }
        hpView.gameObject.SetActive(false);

        ChangeState(MobState.Idle);
        return this;
    }

    public Mob SetMovePath(Vector2[] path)
    {
        moveIdx = 0;
        transform.position = path[moveIdx];
        _movePaths = path;
        return this;
    }

    public void Play()
    {
        ChangeState(MobState.Move);
    }
    

    [Button]
    void Setting()
    {
        ClearMacine();
        SetMacine(MobState.Idle.ToString(),new Mob_Idle());
        SetMacine(MobState.Move.ToString(),new Mob_Move());
        SetMacine(MobState.Dead.ToString(),new Mob_Dead());
        
    }

    public void GetHit(Hit atk)
    {
        if(hpView != null) 
        {
            if(!hpView.gameObject.activeSelf) hpView.gameObject.SetActive(true);
            
            hpView.SetHP(_hp,status.MAX_HP);
        }

    }
}
