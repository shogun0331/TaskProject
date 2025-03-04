using DG.Tweening;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace GB
{
    public class TextAnimation : View
    {
        Text _text;

        [SerializeField] float _duration = 1;
        public bool PlayAutomaticall = true;
        string _defaultText;
        Tween _tweener;
        LocalizationView _localization;

        void Awake()
        {
            _localization = GetComponent<LocalizationView>();
            _text = GetComponent<Text>();
            if (_text != null) _defaultText = _text.text;
        }

        void OnEnable()
        {
            if (_localization != null) Presenter.Bind("Localization", this);
            if (PlayAutomaticall) Play();
        }
        void OnDisable()
        {
            if (_localization != null) Presenter.UnBind("Localization", this);
        }

        void Start()
        {
            if (_localization != null)
            {
                if (PlayAutomaticall) Play();
            }
        }

        public TextAnimation SetText(string text)
        {
            _defaultText = text;
            return this;
        }
        [Button]
        void Play()
        {
            if (_localization != null) _defaultText = LocalizationManager.GetValue(_localization.LocalizationKey);

            if (_tweener != null && _tweener.IsActive()) _tweener.Kill();

            if (_text != null)
            {
                _text.text = string.Empty;
                _tweener = _text.DOText(_defaultText, _duration);
            }
        }

        public override void ViewQuick(string key, IOData data)
        {

            if (_tweener != null && _tweener.IsActive())
            {
                if (_tweener != null && _tweener.IsActive()) _tweener.Kill();
                _defaultText = LocalizationManager.GetValue(_localization.LocalizationKey);
                Play();
            }

        }
    }
}