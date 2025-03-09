
using System.Collections.Generic;
using GB;
using UnityEngine;


public class GameGachaPopup : UIScreen
{
    Player _player;
    GachaTable _table;

    Dictionary<string,RouletteSpin> _spins;

    private void Awake()
    {
        Regist();
        RegistButton();
        _table = GameDataManager.GetTable<GachaTable>();
        _spins = new Dictionary<string, RouletteSpin>();
        _spins["A"] = mGameObject["Spin1"].GetComponent<RouletteSpin>();
        _spins["B"] = mGameObject["Spin2"].GetComponent<RouletteSpin>();
        _spins["C"] = mGameObject["Spin3"].GetComponent<RouletteSpin>();

       
    }

    private void OnEnable()
    {
        _player = ODataBaseManager.Get<Player>("Player0");
        Presenter.Bind("GameGachaPopup",this);

        mTexts["A"].color = _player.Luck >= _table["A"].PriceValue ? Color.white : Color.red;
        mTexts["B"].color = _player.Luck >= _table["B"].PriceValue ? Color.white : Color.red;
        mTexts["C"].color = _player.Luck >= _table["C"].PriceValue ? Color.white : Color.red;

        ODataBaseManager.Bind(this, "LUCK", 
        (value) => 
        { 
             mTexts["A"].color = _player.Luck >= _table["A"].PriceValue ? Color.white : Color.red;
             mTexts["B"].color = _player.Luck >= _table["B"].PriceValue ? Color.white : Color.red;
             mTexts["C"].color = _player.Luck >= _table["C"].PriceValue ? Color.white : Color.red;
        });
    }

    private void OnDisable() 
    {
        ODataBaseManager.UnBind(this,"LUCK");
        Presenter.UnBind("GameGachaPopup", this);

    }

    public void RegistButton()
    {
        foreach(var v in mButtons)
            v.Value.onClick.AddListener(() => { OnButtonClick(v.Key);});
        
    }

    void Roulette(string id)
    {
        if(!_player.CheckUnitCount()) return;
        if(_spins[id].IsPlaying) return;
        if(_player.Luck < _table[id].PriceValue) return;
        _player.SubtracktLUCK(_table[id].PriceValue);

        int rand = Random.Range(0,100); 

        bool isGacha = rand < _table[id].Weight;

        _spins[id].Play(isGacha,()=>
        {
            
            if(isGacha) _player.GachaUnit(id);
        });

    }

    public void OnButtonClick(string key)
    {
        switch(key)
        {
            
            case "A":
            case "B":
            case "C":
            Roulette(key);
            break;

            
            case "Close":
            Close();
            break;

        }
    }
    public override void ViewQuick(string key, IOData data)
    {
         
    }

    public override void Refresh()
    {
            
    }



}