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
        _resultText.color = sucess ? Color.yellow : Color.red;

        _result = result;
        
        _isPlaying = true;

        GB.Timer.Create(0.5f,()=>
        {
            _resultText.transform.localScale = Vector3.one;
            _resultText.transform.DOPunchScale(new Vector3(0.5f,0.5f,0.5f),0.5f).Restart();
            _resultText.gameObject.SetActive(true);
            _panel.SetActive(true);
            _isPlaying = false;
            _result?.Invoke();
        }
        );
    }
    
    
    // Update is called once per frame
    void Update()
    {
        if(!_isPlaying) return;

        _spin.Rotate(-Vector3.forward * _rotSpeed);
    
    
    }
}
