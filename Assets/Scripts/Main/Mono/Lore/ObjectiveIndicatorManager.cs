using NaughtyAttributes;
using UnityEngine;

namespace Main.Mono.Lore
{
    public class ObjectiveIndicatorManager : MonoBehaviour
    {
        [SerializeField] [Required] private Transform _transform;

        public static ObjectiveIndicatorManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void SpawnAt(float x, float y)
        {
            _transform.position = new(x, y, transform.position.z);
            LeanTween.cancel(_transform.gameObject);

            _transform.localScale = Vector2.zero;
            LeanTween
                .scale(_transform.gameObject, Vector2.one, 0.5f)
                .setEaseInOutQuad()
                .setIgnoreTimeScale(true);
        }
    }
}