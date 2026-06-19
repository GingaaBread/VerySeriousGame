using System;
using UnityEngine;

namespace Main.Mono.Tweens
{
    public class ScaleTween : MonoBehaviour
    {
        [SerializeField] private float _from;
        [SerializeField] private float _to;
        [SerializeField] private bool _andBack;
        [SerializeField] private float _time = 0.5f;
        [SerializeField] private bool _applyOnEnable = true;


        [SerializeField] private LeanTweenType _type = LeanTweenType.easeInOutQuart;

        private Vector3 _initialScale;

        private void OnEnable()
        {
            if (_applyOnEnable) Apply();
        }

        private void OnDisable()
        {
            Cancel();
        }

        public void Apply(Action onComplete = null)
        {
            _initialScale = transform.localScale;
            var from = new Vector3(_from, _from);
            var to = new Vector3(_to, _to);

            transform.localScale = from;

            LeanTween
                .scale(gameObject, to, _time)
                .setEase(_type)
                .setIgnoreTimeScale(true)
                .setOnComplete(() =>
                {
                    if (!_andBack)
                    {
                        onComplete?.Invoke();
                        return;
                    }

                    LeanTween
                        .scale(gameObject, from, _time)
                        .setEase(_type)
                        .setIgnoreTimeScale(true)
                        .setOnComplete(onComplete);
                });
        }

        public void Cancel()
        {
            transform.localScale = _initialScale;
            LeanTween.cancel(gameObject);
        }
    }
}