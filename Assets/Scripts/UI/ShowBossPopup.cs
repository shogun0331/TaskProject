using GB;


public class ShowBossPopup : UIScreen
{

    CGame _game;
    NomalWaveTable _nTable;
     

    private void Awake()
    {
        Regist();
        RegistButton();
        _nTable = GameDataManager.GetTable<NomalWaveTable>();
    }

    private void OnEnable()
    {
        Presenter.Bind("ShowBossPopup",this);
        _game = ODataBaseManager.Get<CGame>(DEF.Game);

        mTexts["Wave"].text = "WAVE "+ _game.Wave;
        string bossName = LocalizationManager.GetValue(_nTable[_game.Wave.ToString()].Name);
        mTexts["BossName"].text = string.Format(LocalizationManager.GetValue("21"),bossName);

        int time = (int)_nTable[_game.Wave.ToString()].Dealay;
        mTexts["Time"].text = string.Format(LocalizationManager.GetValue("22"),SetTime(time));
        GB.Timer.Create(2,()=>{Close();});

    }
    string SetTime(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }

    private void OnDisable() 
    {
        Presenter.UnBind("ShowBossPopup", this);

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