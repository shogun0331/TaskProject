using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace GB
{
    public class PanelFade : MonoBehaviour
    {
        public enum FadeType { FadeIn, FadeOut };
        CanvasGroup _group;
        Image _img;
        [SerializeField] float _fadeInDuration = 1;
        [SerializeField] float _fadeOutDuration = 1;

        [SerializeField] FadeType _fadeType = FadeType.FadeOut;

        Tween _tweener;

        public bool PlayAutomaticall = true;

        void Awake()
        {
            _group = GetComponent<CanvasGroup>();
            _img = GetComponent<Image>();
        }

        void OnEnable()
        {
            if (PlayAutomaticall)
            {
                if (_fadeType == FadeType.FadeIn) FadeIn();
                else FadeOut();
            }
        }


        public void FadeIn()
        {
            if (_tweener != null && _tweener.IsActive()) _tweener.Kill();
            if (_group != null) _tweener = _group.DOFade(1, _fadeInDuration);
            else if (_img != null) _tweener = _img.DOFade(1, _fadeInDuration);
        }


        public void FadeOut()
        {
            if (_tweener != null && _tweener.IsActive()) _tweener.Kill();
            if (_group != null) _tweener = _group.DOFade(0, _fadeInDuration);
            else if (_img != null) _tweener = _img.DOFade(0, _fadeInDuration);

        }

    }
}