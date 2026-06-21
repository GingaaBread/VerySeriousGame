using Lean.Pool;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Utility.Polish
{
    public class IndicatorText : MonoBehaviour
    {
        [Required] [SerializeField] private Animator _animator;
        [Required] [SerializeField] private TMP_Text _text;
        [Required] [SerializeField] private RectTransform _rectTransform;

        public void Render(Vector2 position, string indicator)
        {
            _text.text = indicator;
            _rectTransform.position = position;
            _animator.Play("Indicator");
        }

        public void MarkCompletion()
        {
            LeanPool.Despawn(this);
        }
    }
}