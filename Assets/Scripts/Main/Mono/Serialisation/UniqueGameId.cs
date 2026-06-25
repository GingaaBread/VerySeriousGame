using System;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;

namespace Main.Mono.Serialisation
{
    [DisallowMultipleComponent]
    public class UniqueGameId : MonoBehaviour
    {
        [field: SerializeField] [field: ReadOnly] public string Identifier { private set; get; } = string.Empty;

        private void Regenerate()
        {
            Identifier = Guid.NewGuid().ToString();
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(Identifier) || HasDuplicateIdInScene()) Regenerate();
        }

        private bool HasDuplicateIdInScene()
        {
            return FindObjectsByType<UniqueGameId>(FindObjectsInactive.Include, FindObjectsSortMode.None)
                .Any(other => other.gameObject != gameObject && other.Identifier == Identifier);
        }
#endif
    }
}