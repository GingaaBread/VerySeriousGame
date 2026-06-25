using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Utility.Essentials
{
    public class EssentialsLoader : MonoBehaviour
    {
        private static bool _isLoading;
        [SerializeField] private UnityEvent _onExistsAlready;

        private async void Awake()
        {
            if (SpawnedEssentials.Exists || _isLoading)
            {
                _onExistsAlready?.Invoke();
                return;
            }

            _isLoading = true;

            var handle = Addressables.InstantiateAsync("spawned-essentials");
            await handle.Task;
        }
    }
}