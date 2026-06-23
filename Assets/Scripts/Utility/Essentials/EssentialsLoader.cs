using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Utility.Essentials
{
    public class EssentialsLoader : MonoBehaviour
    {
        private static bool _isLoading;
        [Inject] private IObjectResolver _resolver;

        private async void Awake()
        {
            if (SpawnedEssentials.Exists || _isLoading) return;
            _isLoading = true;

            var handle = Addressables.InstantiateAsync("spawned-essentials");
            var essentials = await handle.Task;
            var injected = essentials.GetComponent<SpawnedEssentials>().Injected;

            foreach (var i in injected)
            {
                _resolver.InjectGameObject(i);
            }

            _resolver.InjectGameObject(essentials);
        }
    }
}