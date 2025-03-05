using GB;

public class GameScene : UIScreen
{

    CGame _game;

    private void Awake()
    {
        Regist();
        RegistButton();



    }
    void Start()
    {
        if (ODataBaseManager.Contains(DEF.Game))
        {
            _game = ODataBaseManager.Get<CGame>(DEF.Game);
        }
        else
        {
            ODataBaseManager.Bind(this, DEF.Game, (value =>
            {
                _game = value.Get<CGame>();
                ODataBaseManager.UnBind(this, DEF.Game);
            }));
        }
    }

    private void OnEnable()
    {


        Presenter.Bind("GameScene", this);
    }

    private void OnDisable()
    {
        Presenter.UnBind("GameScene", this);

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
                _game.Summon();
                break;
            case "Gacha":
                _game.Gacha();
                break;
            case "Myth":
                _game.Myth();
                break;

        }
    }
    public override void ViewQuick(string key, IOData data)
    {
        switch (key)
        {
            case "Time":
                SetTime(data.Get<int>());
                break;
            case "Wave":
                mTexts["Wave"].text = "Wave " + data.Get<int>();
                break;

            case "WaveCount":
                SetWaveCount(data.Get<int>());
                break;
        }
    }

    void SetWaveCount(int count)
    {
        mTexts["WaveCount"].text = string.Format("{0} / {1}", count, DEF.MOB_MAXCOUNT);
    }

    void SetTime(int time)
    {
        int minutes = time / 60;
        int seconds = time % 60;
        mTexts["Time"].text = $"{minutes:D2}:{seconds:D2}";
    }

    public override void Refresh()
    {

    }



}