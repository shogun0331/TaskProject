using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class RouletteSpin : MonoBehaviour
{
    [SerializeField] Transform _spin;
    [SerializeField] Text _resultText;
    [SerializeField] GameObject _panel;
    float _rotSpeed = 5000;
    bool _isPlaying;
    float _time = 0;
    Action _result;
    void OnEnable()
    {
        _resultText.gameObject.SetActive(false);
        
    }

    public void Play(bool sucess,Action result)
    {
        if(_isPlaying)return;
        _panel.SetActive(false);
        _resultText.gameObject.SetActive(false);
        _resultText.text = sucess ?  GB.LocalizationManager.GetValue("18")  : GB.LocalizationManager.GetValue("19");
        _result = result;
        _time = 0;
        _isPlaying = true;

        GB.Timer.Create(2,()=>
        {
            _resultText.transform.localScale = Vector3.one;
            _resultText.transform.DOPunchScale(new Vector3(0.2f,0.2f,0.2f),0.5f).Restart();
            _resultText.gameObject.SetActive(true);}
        );
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if(!_isPlaying) return;

        _spin.Rotate(-Vector3.forward * _rotSpeed);
        _time += GBTime.GetDeltaTime(DEF.T_GAME);

        if(_time >  2)
        {
            _panel.SetActive(true);
            _isPlaying = false;
            _result?.Invoke();
        }
    }
}
