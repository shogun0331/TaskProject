using GB;


public class WaveShowPopup : UIScreen
{

    private void Awake()
    {
        Regist();
        RegistButton();
    }

    private void OnEnable()
    {
        Presenter.Bind("WaveShowPopup",this);
        var game = ODataBaseManager.Get<CGame>(DEF.Game);
        mTexts["Wave"].text = "Wave " +  game.Wave;
        Timer.Create(1,()=>{Close();});
    }

    private void OnDisable() 
    {
        Presenter.UnBind("WaveShowPopup", this);

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