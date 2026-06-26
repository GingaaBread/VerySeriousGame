using System.Collections.Generic;
using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class SerialisationService
    {
        private readonly SerialisationStatus _serialisationStatus = new();

        public void MarkAsDestroyed(string identifier)
        {
            if (_serialisationStatus.DestroyedObjects.Contains(identifier)) return;

            Debug.Log($"Marking {identifier} as destroyed");
            _serialisationStatus.DestroyedObjects.Add(identifier);
        }

        public List<string> GetAllDestroyed() => _serialisationStatus.DestroyedObjects;

        public void ResetAllDestroyed()
        {
            Debug.Log("Clearing all objects that have been marked as destroyed");
            _serialisationStatus.DestroyedObjects.Clear();
        }
    }
}