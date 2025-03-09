using GB;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine;

public class ResultPopup : UIScreen
{
    Player _player1;
    Player _player2;
    Slider _slider;
    

    private void Awake()
    {
        Regist();
        RegistButton();
        _slider = mGameObject["Slider"].GetComponent<Slider>();
    }

    private void OnEnable()
    {
        Presenter.Bind("ResultPopup",this);

        _player1 = ODataBaseManager.Get<Player>("Player0");
        _player2 = ODataBaseManager.Get<Player>("Player1");


        var game = ODataBaseManager.Get<CGame>(DEF.Game);

        float per = (float)_player1.mobDeadCount /  ((float)(_player1.mobDeadCount + _player2.mobDeadCount));
        _slider.value = per;

        int myPercent = (int)(per * 100);

        mTexts["MyContri"].text = string.Format("{0}%", myPercent);
        mTexts["FrContri"].text = string.Format("{0}%",100 - myPercent);
        
        _player1.mobDeadCount.GBLog("My");
        _player2.mobDeadCount.GBLog("Fr");

        mTexts["Wave"].text = game.Wave.ToString(); 
        mGameObject["Board"].transform.localScale = new Vector3(1,0,1);
        mGameObject["Board"].transform.DOScaleY(1,0.5f).Restart();
    }

    private void OnDisable() 
    {
        Presenter.UnBind("ResultPopup", this);

    }

    public override void BackKey()
    {
         UIManager.ChangeScene("GameScene");
    }

    public void RegistButton()
    {
        foreach(var v in mButtons)
            v.Value.onClick.AddListener(() => { OnButtonClick(v.Key);});
        
    }

    public void OnButtonClick(string key)
    {
        switch(key)
        {
            case "ReGame":
            UIManager.ChangeScene("GameScene");
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