using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Utility.Polish
{
    public class IndicatorManager : MonoBehaviour
    {
        [SerializeField] [Required] private Canvas _canvas;
        [SerializeField] [Required] private IndicatorText _prefab;

        public static IndicatorManager Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "IndicatorManager singleton already exists");
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;
        }

        public void RequireAt(string indicator, Vector2 position)
        {
            var pooledInstance = LeanPool.Spawn(_prefab, _canvas.transform);
            var randomOffset = new Vector2(
                Random.Range(-0.8f, 0.8f),
                Random.Range(-0.8f, 0.8f)
            );
            var offsetPos = Random.Range(0, 2) is 0 ? randomOffset : -randomOffset;
            pooledInstance.Render(position + offsetPos, indicator);
        }
    }
}