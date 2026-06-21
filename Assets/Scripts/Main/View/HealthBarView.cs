using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    public class HealthBarView : MonoBehaviour
    {
        [Required] [SerializeField] private Image _fillImage;
        [SerializeField] private LeanTweenType _tweenType = LeanTweenType.easeInOutCubic;
        [SerializeField] private float _updateTime = 0.3f;

        public void RenderInitialPercentage(float percentage)
        {
            _fillImage.fillAmount = percentage;
        }

        public void UpdatePercentage(float percentage)
        {
            LeanTween.cancel(gameObject);
            var current = _fillImage.fillAmount;

            var delta = Mathf.Abs(percentage - current);
            var duration = _updateTime * delta;

            LeanTween
                .value(gameObject, current, percentage, duration)
                .setOnUpdate(val => _fillImage.fillAmount = val)
                .setEase(_tweenType);
        }
    }
}