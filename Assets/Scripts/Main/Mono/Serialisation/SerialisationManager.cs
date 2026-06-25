using System.Collections;
using System.Linq;
using Main.Service;
using UnityEngine;
using VContainer;

namespace Main.Mono.Serialisation
{
    public class SerialisationManager : MonoBehaviour
    {
        [Inject] private readonly SerialisationService _serialisationService;

        private void Awake()
        {
            StartCoroutine(WaitForService());
        }

        private IEnumerator WaitForService()
        {
            yield return new WaitUntil(() => _serialisationService != null);
            DeleteAllDestroyed();
        }

        private void DeleteAllDestroyed()
        {
            var destroyed = _serialisationService.GetAllDestroyed();
            var objs = FindObjectsByType<UniqueGameId>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);

            foreach (var obj in objs)
            {
                if (destroyed.All(id => obj.Identifier != id)) continue;

                Debug.Log($"{obj} was found among the destroyed serialised objects. Deactivating...");
                obj.gameObject.SetActive(false);
            }
        }
    }
}