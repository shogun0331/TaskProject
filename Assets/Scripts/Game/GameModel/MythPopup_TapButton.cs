using UnityEngine;
using UnityEngine.UI;

public class MythPopup_TapButton : MonoBehaviour
{
    [SerializeField] Image _icon;
    [SerializeField] Text _progressText;
    
    public void SetButton(string progressText,Sprite icon)
    {
        _progressText.text = progressText;
        _icon.sprite = icon;
    }

}
