using UnityEngine;
using UnityEngine.AddressableAssets;
using Utility.Scopes;
using VContainer.Unity;

namespace Utility.Essentials
{
    public class EssentialsLoader : MonoBehaviour
    {
        private static bool _isLoading;

        private async void Awake()
        {
            if (SpawnedEssentials.Exists || _isLoading) return;
            _isLoading = true;

            var handle = Addressables.InstantiateAsync("spawned-essentials");
            var essentials = await handle.Task;
            var injected = essentials.GetComponent<SpawnedEssentials>().Injected;
            var scope = FindAnyObjectByType<BaseLifetimeScope>();

            foreach (var i in injected)
            {
                scope.Container.InjectGameObject(i);
            }

            scope.Container.InjectGameObject(essentials);
        }
    }
}