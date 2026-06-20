using Lean.Pool;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;
using Utility.Polish;

namespace Main.Mono.Collected_Items
{
    public class SlashManager : MonoBehaviour
    {
        [Required] [SerializeField] private Canvas _canvas;
        [Required] [SerializeField] private SlashImage _prefab;

        public static SlashManager Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "SlashManager singleton already exists");
            Instance = this;
        }

        private void OnDisable()
        {
            Instance = null;
        }

        public void RequireAt(Vector2 position)
        {
            var pooledInstance = LeanPool.Spawn(_prefab, _canvas.transform);
            pooledInstance.transform.position = position;
            pooledInstance.Render();
        }
    }
}