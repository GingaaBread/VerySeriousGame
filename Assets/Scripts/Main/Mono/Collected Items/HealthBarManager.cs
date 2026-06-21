using Lean.Pool;
using Main.View;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Main.Mono.Collected_Items
{
    public class HealthBarManager : MonoBehaviour
    {
        [Required] [SerializeField] private HealthBarView _prefab;
        [Required] [SerializeField] private Canvas _canvas;

        public static HealthBarManager Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "HealthBarManager singleton already exists");
            Instance = this;
        }

        public HealthBarView SpawnNewAt(Vector2 position, float initialPercentage)
        {
            var pooledInstance = LeanPool.Spawn(_prefab, _canvas.transform);
            pooledInstance.transform.position = position;
            pooledInstance.RenderInitialPercentage(initialPercentage);

            return pooledInstance;
        }
    }
}