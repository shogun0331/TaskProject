using GB;
using QuickEye.Utility;
using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MythPopup : UIScreen
{
    [SerializeField] UnityDictionary<string, Sprite> _mythIcons;
    MythTable _table;
    Player _player;
    MythPopup_TapButton[] _taps;

    string _target;

    private void Awake()
    {
        Regist();
        RegistButton();
        _table = GameDataManager.GetTable<MythTable>();
        _taps = mGameObject["ScrollParent"].transform.GetComponentsInChildren<MythPopup_TapButton>(true);
    }



    private void OnEnable()
    {
        _player = ODataBaseManager.Get<Player>("Player0");
        Presenter.Bind("MythPopup", this);

        _target = string.Empty;

        for (int i = 0; i < _taps.Length; ++i)
        {
            bool isActive = i < _player.UnitIDDatas[UnitRank.S].Count;
            if (i == 0)
                _target = _player.UnitIDDatas[UnitRank.S][0].ID;

            _taps[i].gameObject.SetActive(isActive);
            if (isActive)
            {
                string text = string.Format(LocalizationManager.GetValue("17"), Progress(_player.UnitIDDatas[UnitRank.S][i].ID));
                _taps[i].SetButton(text, _mythIcons[_player.UnitIDDatas[UnitRank.S][i].ID]);
            }
        }
        SetTap(_target);


    }

    int Progress(string unitName)
    {
        var data = GetTableUnits(unitName);
        float totalCnt = 0;
        float cnt = 0;
        foreach (var v in data)
        {
            int unitCount = _player.GetUnitCount(v.Key);
            if (unitCount > v.Value) cnt += v.Value;
            else cnt += unitCount;
            totalCnt += v.Value;
        }

        return (int)((cnt / totalCnt) * 100f);
    }

    void SetTap(string unitID)
    {
        //신화 소환 조건 충족 - 소환 버튼
        mButtons["Summon"].gameObject.SetActive(_player.CheckMythSummon(unitID));
        //Unit 아이콘 적용
        mImages["UnitIcon"].sprite = _mythIcons[unitID];

        // 번역 아이디 적용
        mTexts["UnitName"].text = LocalizationManager.GetValue(_table[unitID].Name);

        //소환 재료 유닛 표시
        var data = GetTableUnits(unitID);
        bool isMergeUnit1 = !string.IsNullOrEmpty(_table[unitID].A_ID);
        bool isMergeUnit2 = !string.IsNullOrEmpty(_table[unitID].B_ID);
        bool isMergeUnit3 = !string.IsNullOrEmpty(_table[unitID].C_ID);
        bool isMergeUnit4 = !string.IsNullOrEmpty(_table[unitID].D_ID);

        List<UISkinner> skinners = new List<UISkinner>();
        if (isMergeUnit1)
        {
            mImages["MergeUnit1"].sprite = _mythIcons[_table[unitID].A_ID];
            skinners.Add(mSkinner["MergeUnit1"]);
        }
        if (isMergeUnit2)
        {
            mImages["MergeUnit2"].sprite = _mythIcons[_table[unitID].B_ID];
            skinners.Add(mSkinner["MergeUnit2"]);
        }
        if (isMergeUnit3)
        {
            mImages["MergeUnit3"].sprite = _mythIcons[_table[unitID].C_ID];
            skinners.Add(mSkinner["MergeUnit3"]);
        }
        if (isMergeUnit4)
        {
            mImages["MergeUnit4"].sprite = _mythIcons[_table[unitID].D_ID];
            skinners.Add(mSkinner["MergeUnit4"]);
        }

        mSkinner["MergeUnit1"].gameObject.SetActive(isMergeUnit1);
        mSkinner["MergeUnit2"].gameObject.SetActive(isMergeUnit2);
        mSkinner["MergeUnit3"].gameObject.SetActive(isMergeUnit3);
        mSkinner["MergeUnit4"].gameObject.SetActive(isMergeUnit4);

        int skinerCnt = 0;
        foreach (var v in data)
        {
            int count = _player.GetUnitCount(v.Key);
            for (int i = 0; i < v.Value; ++i)
            {
                skinners[skinerCnt].SetSkin(count > i ? "On" : "Off");
                skinerCnt++;
            }
        }


    }

    Dictionary<string, int> GetTableUnits(string unitName)
    {
        Dictionary<string, int> data = new Dictionary<string, int>();
        if (!string.IsNullOrEmpty(_table[unitName].A_ID))
        {
            data[_table[unitName].A_ID] = 1;
        }

        if (!string.IsNullOrEmpty(_table[unitName].B_ID))
        {
            string id = _table[unitName].B_ID;
            if (data.ContainsKey(id)) data[id]++;
            else data[id] = 1;
        }

        if (!string.IsNullOrEmpty(_table[unitName].C_ID))
        {
            string id = _table[unitName].C_ID;
            if (data.ContainsKey(id)) data[id]++;
            else data[id] = 1;
        }

        if (!string.IsNullOrEmpty(_table[unitName].D_ID))
        {
            string id = _table[unitName].D_ID;
            if (data.ContainsKey(id)) data[id]++;
            else data[id] = 1;
        }

        return data;

    }



    private void OnDisable()
    {
        Presenter.UnBind("MythPopup", this);

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
            case "Summon":
                _player.SummonMythMerge(_target);
                Close();
                break;

            case "Close":
                Close();
                break;
            default:
                _target = key;
                SetTap(key);
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