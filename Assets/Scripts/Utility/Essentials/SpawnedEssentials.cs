using UnityEngine;
using VContainer.Unity;

namespace Utility.Essentials
{
    public class SpawnedEssentials : MonoBehaviour
    {
        [SerializeField] private GameObject[] _autoInjectGameObjects;

        public static bool Exists { private set; get; }

        private void Awake()
        {
            if (Exists) return;

            DontDestroyOnLoad(gameObject);
            Exists = true;

            var scope = LifetimeScope.Find<GameLifetimeScope>();
            foreach (var autoInjectGameObject in _autoInjectGameObjects)
            {
                foreach (var component in autoInjectGameObject.GetComponents<MonoBehaviour>())
                {
                    scope.Container.Inject(component);
                }
            }
        }
    }
}