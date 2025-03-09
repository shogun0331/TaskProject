using GB;
using QuickEye.Utility;

using UnityEngine;


public class CongratulationsPopup : UIScreen
{

    

    private void Awake()
    {
        Regist();
        RegistButton();
    }

    
    Unit _unit;


    private void OnEnable()
    {
        Presenter.Bind("CongratulationsPopup",this);

        _unit = ODataBaseManager.Get<Unit>("CongratulationUnit");

        if(_unit.rank == UnitRank.A) mTexts["Info"].text = LocalizationManager.GetValue("7");
        else if(_unit.rank == UnitRank.S) mTexts["Info"].text = LocalizationManager.GetValue("8");

        mSkinner["Skin"].SetSkin(_unit.ID);
        mSkinner["Skin"].Apply();
        GB.Timer.Create(2,()=>
        {
            Close();
        });
    }

    private void OnDisable() 
    {
        Presenter.UnBind("CongratulationsPopup", this);

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