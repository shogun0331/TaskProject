using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;
using QuickEye.Utility;
using System;

namespace GB
{
    public class ButtonAnimation : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
    {

        public enum BtnActionStyle { Linear, Punch, Shake }
        public enum BtnEventType { Up, Down }


        [HideInInspector] public UIButtonData ButtonData = new UIButtonData();

        bool _isDown;

        Vector3 _originScale;

        [SerializeField] USkin _ButtonUpSkinner;
        [SerializeField] USkin _ButtonDownSkinner;

        public UnityEvent _ClickEvent;
        public UnityEvent _DownEvent;
        public UnityEvent _UpEvent;

        Tween _tweener;


        void Start()
        {
            _originScale = transform.localScale;

            
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _isDown = false;
            _ClickEvent?.Invoke();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (_isDown) return;
            _isDown = true;
            if (_ButtonDownSkinner != null) _ButtonDownSkinner.Apply();
            _DownEvent?.Invoke();
            ButtonDownTween();


        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (!_isDown) return;
            if (_ButtonUpSkinner != null) _ButtonUpSkinner.Apply();
            _isDown = false;
            _UpEvent?.Invoke();
            ButtonUpTween();


        }



        void ButtonUpTween()
        {
            if (ButtonData.BtnState[BtnEventType.Up] == false) return;
            if (_tweener != null && _tweener.IsActive())
            {
                _tweener.Kill();
            }

            var eventType = BtnEventType.Up;
            _tweener = PlayTween(eventType);
            _tweener.OnComplete(() =>
            {
                transform.localScale = _originScale;
            });

        }



        void ButtonDownTween()
        {
            if (ButtonData.BtnState[BtnEventType.Down] == false) return;

            if (_tweener != null && _tweener.IsActive())
            {
                _tweener.Kill();
                transform.localScale = _originScale;
            }

            var eventType = BtnEventType.Down;

            _tweener = PlayTween(eventType);

        }

        Tween PlayTween(BtnEventType eventType)
        {
            float duration = ButtonData.DurationValues[eventType];
            var style = ButtonData.BtnStyle[eventType];
            float power = ButtonData.PowerValues[eventType];
            switch (style)
            {
                case BtnActionStyle.Linear:
                    return transform.DOScale(power, duration);

                case BtnActionStyle.Punch:
                    return transform.DOPunchScale(new Vector3(power, power, power), duration);


                case BtnActionStyle.Shake:
                    return transform.DOShakeScale(duration, power);

            }

            return null;

        }


        [Serializable]
        public class UIButtonData
        {
            [HideInInspector]
            public UnityDictionary<BtnEventType, bool> BtnState = new UnityDictionary<BtnEventType, bool>
            {
                {BtnEventType.Up,true},
                {BtnEventType.Down,true},

            };

            [HideInInspector]
            public UnityDictionary<BtnEventType, BtnActionStyle> BtnStyle = new UnityDictionary<BtnEventType, BtnActionStyle>
            {
                {BtnEventType.Up,BtnActionStyle.Linear},
                {BtnEventType.Down,BtnActionStyle.Linear},

            };

            [HideInInspector]
            public UnityDictionary<BtnEventType, float> DurationValues = new UnityDictionary<BtnEventType, float>
            {
                {BtnEventType.Up,0.2f},
                {BtnEventType.Down,0.2f},

            };
      

            [HideInInspector]
            public UnityDictionary<BtnEventType, float> PowerValues = new UnityDictionary<BtnEventType, float>
            {
                {BtnEventType.Up,1},
                {BtnEventType.Down,1},
            };

        }

    }
}