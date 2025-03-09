using DG.Tweening;
using GB;
using UnityEngine;


public class BossClearPopup : UIScreen
{

    

    private void Awake()
    {
        Regist();
        RegistButton();
    }

    private void OnEnable()
    {
        Presenter.Bind("BossClearPopup",this);
        GB.Timer.Create(2,()=>{Close();});

        mGameObject["Icon"].transform.localScale = Vector3.one;
        mGameObject["Icon"].transform.DOPunchScale(new Vector3(0.5f,0.5f,0.5f),0.2f).Restart();
    }

    private void OnDisable() 
    {
        Presenter.UnBind("BossClearPopup", this);

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

        }
    }
    public override void ViewQuick(string key, IOData data)
    {
         
    }

    public override void Refresh()
    {
            
    }



}