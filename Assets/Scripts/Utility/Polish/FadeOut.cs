using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

namespace Utility.Polish
{
    public class FadeOut : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Light2D _light;
        [Min(0.1f)] [SerializeField] private float _fadeOutTime = 1f;
        [SerializeField] private LeanTweenType _tweenType;

        [SerializeField] private UnityEvent _onComplete;

        public void Apply()
        {
            if (_spriteRenderer == null && _light == null)
            {
                Debug.LogError("A fade out requires either a light or sprite renderer");
                return;
            }

            var spriteColor = _spriteRenderer != null ? _spriteRenderer.color : default;
            var lightColor = _light != null ? _light.color : default;

            var currentAlpha = _light != null ? lightColor.a : spriteColor.a;

            LeanTween
                .value(gameObject, currentAlpha, 0f, _fadeOutTime)
                .setOnUpdate(val =>
                {
                    if (_spriteRenderer != null)
                    {
                        spriteColor.a = val;
                        _spriteRenderer.color = spriteColor;
                    }

                    if (_light != null)
                    {
                        lightColor.a = val;
                        _light.color = lightColor;
                    }
                })
                .setEase(_tweenType)
                .setOnComplete(() => _onComplete?.Invoke());
        }
    }
}