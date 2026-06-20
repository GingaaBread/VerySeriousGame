using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Utility.Polish
{
    /// <summary>
    ///     This UI image renders a random slash texture using a random transform.
    /// </summary>
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(Animator))]
    public class SlashImage : MonoBehaviour
    {
        [InfoBox("The non-empty array of random slash sprites, out of which one will be rendered")] [SerializeField]
        private Sprite[] _textures;

        [field: Required]
        [field: SerializeField] public Image Image { get; private set; }

        private Animator _animator;
        private RectTransform _rectTransform;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _rectTransform = GetComponent<RectTransform>();
        }

        /// <summary>
        ///     Renders the slash image using a random slash sprite and a random scale and rotation.
        /// </summary>
        public void Render()
        {
            Image.sprite = _textures[Random.Range(0, _textures.Length)];

            var scale = Random.Range(0.3f, 1f);
            _rectTransform.localScale = new(scale, scale);

            var rotation = _rectTransform.eulerAngles;
            rotation = new(rotation.x, rotation.y, Random.Range(-90, 90));
            _rectTransform.eulerAngles = rotation;

            _animator.Play("Slash");
        }

        public void MarkCompletion()
        {
            _animator.Play("Idle");
            LeanPool.Despawn(this);
        }
    }
}