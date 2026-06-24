using UnityEngine;
using UnityEngine.AddressableAssets;

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
            await handle.Task;
        }
    }
}