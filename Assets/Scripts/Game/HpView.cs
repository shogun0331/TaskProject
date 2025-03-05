using UnityEngine;
using UnityEngine.UI;

public class HpView : MonoBehaviour
{
    [SerializeField] Slider _slider;

    public void SetHP(int hp, int mxHp)
    {
        _slider.value = (float)hp /(float)mxHp;
    }
}
