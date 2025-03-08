
using UnityEngine;
using GB;
using UnityEngine.UI;

public class BinderCurrency : MonoBehaviour
{
    [SerializeField] Text _goldText;
    [SerializeField] Text _luckText;
    [SerializeField] Text _unitText;

    void OnEnable()
    {

        if (_goldText != null)
        {
            if (ODataBaseManager.Contains("GOLD")) _goldText.text = GameUtil.FormatKmt(ODataBaseManager.Get<int>("GOLD"));
            ODataBaseManager.Bind(this, "GOLD", (value) => { _goldText.text = GameUtil.FormatKmt(value.Get<int>()); });
        }

        if (_luckText != null)
        {
            if (ODataBaseManager.Contains("LUCK")) _luckText.text = GameUtil.FormatKmt(ODataBaseManager.Get<int>("LUCK"));
            ODataBaseManager.Bind(this, "LUCK", (value) => { _luckText.text = GameUtil.FormatKmt(value.Get<int>()); });
        }

        if (_unitText != null)
        {
            
            if (ODataBaseManager.Contains("UnitCount")) 
            {
                int cnt = ODataBaseManager.Get<int>("UnitCount");
                _unitText.text = string.Format("{0} / 20",cnt);
                _unitText.color = cnt >= 20 ? Color.red : Color.white;
            }
            ODataBaseManager.Bind(this, "UnitCount", (value) => 
            {  
                int cnt = value.Get<int>();
                _unitText.text = string.Format("{0} / 20",cnt);
                _unitText.color = cnt >= 20 ? Color.red : Color.white;
            });
        }




    }
    void OnDisable()
    {
        ODataBaseManager.UnBind(this,"UnitCount");
        ODataBaseManager.UnBind(this,"LUCK");
        ODataBaseManager.UnBind(this,"GOLD");

    }

}
