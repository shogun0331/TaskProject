using GB;
using UnityEngine;
using System.Linq;
using System.Collections.Generic;


public class UpgradePopup : UIScreen
{
    Player _player;
    UpgradeTable _table;
    Dictionary<string, bool> _isMaxLevels = new Dictionary<string, bool>();

    private void Awake()
    {
        Regist();
        RegistButton();
        _table = GameDataManager.GetTable<UpgradeTable>();
    }

    private void OnEnable()
    {
        _player = ODataBaseManager.Get<Player>("Player0");

        ODataBaseManager.Bind(this, "GOLD",(value)=>
        {
            Refresh();
        });

        ODataBaseManager.Bind(this, "LUCK",(value)=>
        {
            Refresh();
        });



        Presenter.Bind("UpgradePopup", this);
        Refresh();
    }


    private void OnDisable()
    {
        Presenter.UnBind("UpgradePopup", this);
    }



    public void RegistButton()
    {
        foreach (var v in mButtons)
            v.Value.onClick.AddListener(() => { OnButtonClick(v.Key); });
    }

    public void OnButtonClick(string key)
    {
        switch (key)
        {
            case "A":
            Upgrade("C");
            break;

            case "B":
            Upgrade("A");
            break;

            case "C":
            Upgrade("S");
            break;

            case "D":
            Upgrade("Summon");
            break;
            case "Close":
            Close();
            break;

        }
    }
    public override void ViewQuick(string key, IOData data)
    {

    }

    public void Upgrade(string id)
    {
        if(_isMaxLevels[id]) return;
        if(!CheckCurrency(id)) return;
        string cType = CurrencyType(id);
        if(cType == "GOLD")_player.SubtractGold(GetPrice(id));
        else if(cType == "LUCK")_player.SubtracktLUCK(GetPrice(id));
        _player.Upgrade(id);
        Refresh();
    }

    public bool CheckCurrency(string id)
    {
        string cType = CurrencyType(id);
        if(cType == null) return false;

        if(cType == "GOLD")
        {
             if(_player.Gold >= GetPrice(id))
             return true;
        }

        else if(cType == "LUCK")
        {
             if(_player.Luck >= GetPrice(id))
             return true;
        }

        return false;

    }

    public string CurrencyType(string id)
    {
        var list = _table.Datas.Where(v => v.UpgradeType == id).ToList();
        if(list == null) return null;;
        var prob = list.FirstOrDefault(v => v.Level == _player.Levels[id] + 1);
        if(prob == null) return null;
        return prob.PrieceID;

    }

    public int GetPrice(string id)
    {
        var list = _table.Datas.Where(v => v.UpgradeType == id).ToList();
        if(list == null) return -1;
        var prob = list.FirstOrDefault(v => v.Level == _player.Levels[id] + 1);
        if(prob == null) return -1;
        return prob.PriceValue;
    }




    public override void Refresh()
    {
        if (_player == null) return;

        int cLevel = _player.Levels["C"];
        int aLevel = _player.Levels["A"];
        int sLevel = _player.Levels["S"];
        int summonLevel = _player.Levels["Summon"];

        mTexts["A"].text = "Lv." + cLevel;
        mTexts["B"].text = "Lv." + aLevel;
        mTexts["C"].text = "Lv." + sLevel;
        mTexts["D"].text = "Lv." + summonLevel;

        var cList = _table.Datas.Where(v => v.UpgradeType == "C").ToList();
        var aList = _table.Datas.Where(v => v.UpgradeType == "A").ToList();
        var sList = _table.Datas.Where(v => v.UpgradeType == "S").ToList();
        var summonList = _table.Datas.Where(v => v.UpgradeType == "Summon").ToList();

        _isMaxLevels["C"] = false;
        _isMaxLevels["A"] = false;
        _isMaxLevels["S"] = false;
        _isMaxLevels["Summon"] = false;

        var prob = cList.FirstOrDefault(v => v.Level == cLevel + 1);
        if (prob != null) 
        {
            mTexts["A_Price"].text = prob.PriceValue.ToString("N0");
            if( _player.Gold >= prob.PriceValue) mTexts["A_Price"].color = Color.white;
            else mTexts["A_Price"].color = Color.red;
        }
        else
        {
             mTexts["A_Price"].color = Color.white;
            _isMaxLevels["C"] = true;
            mTexts["A_Price"].text = "MAX";
            mButtons["A"].interactable = false;
        }

        prob = aList.FirstOrDefault(v => v.Level == aLevel + 1);
        if (prob != null) 
        {
            mTexts["B_Price"].text = prob.PriceValue.ToString("N0");
            if( _player.Gold >= prob.PriceValue) mTexts["B_Price"].color = Color.white;
            else mTexts["B_Price"].color = Color.red;
        }
        else
        {
             mTexts["B_Price"].color = Color.white;
            _isMaxLevels["A"] = true;
            mTexts["B_Price"].text = "MAX";
            mButtons["B"].interactable = false;
        }

        prob = sList.FirstOrDefault(v => v.Level == sLevel + 1);
        if (prob != null) 
        {
            mTexts["C_Price"].text = prob.PriceValue.ToString("N0");
            if( _player.Luck >= prob.PriceValue) mTexts["C_Price"].color = Color.white;
            else mTexts["C_Price"].color = Color.red;
        }
        else
        {
            mTexts["C_Price"].color = Color.white;
            _isMaxLevels["S"] = true;
            mTexts["C_Price"].text = "MAX";
            mButtons["C"].interactable = false;
        }

        prob = summonList.FirstOrDefault(v => v.Level == summonLevel + 1);
        if (prob != null)
        {
            mTexts["D_Price"].text = prob.PriceValue.ToString("N0");
            if( _player.Gold >= prob.PriceValue) mTexts["D_Price"].color = Color.white;
            else mTexts["D_Price"].color = Color.red;
        } 
        else
        {
            mTexts["D_Price"].color = Color.white;
            _isMaxLevels["Summon"] = true;
            mTexts["D_Price"].text = "MAX";
            mButtons["D"].interactable = false;
        }
    }



}