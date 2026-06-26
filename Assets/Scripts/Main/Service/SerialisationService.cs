using System;
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
        public event Action<int, int> OnProgressUpdate;

        public (int, int) GetCurrentProgress() => (_serialisationStatus.DestroyedObjects.Count,
            _serialisationStatus.TotalDestroyedObjectCount);

        public void MarkAsDestroyed(string identifier)
        {
            if (_serialisationStatus.DestroyedObjects.Contains(identifier)) return;

            Debug.Log($"Marking {identifier} as destroyed");
            _serialisationStatus.DestroyedObjects.Add(identifier);
            TriggerUpdate();
        }

        public List<string> GetAllDestroyed() => _serialisationStatus.DestroyedObjects;

        public void ResetAllDestroyed()
        {
            Debug.Log("Clearing all objects that have been marked as destroyed");
            _serialisationStatus.DestroyedObjects.Clear();
            TriggerUpdate();
        }

        public void RegisterTotalAmount(int amount)
        {
            _serialisationStatus.TotalDestroyedObjectCount = amount;
            TriggerUpdate();
        }

        private void TriggerUpdate()
        {
            OnProgressUpdate?.Invoke(_serialisationStatus.DestroyedObjects.Count,
                _serialisationStatus.TotalDestroyedObjectCount);
        }
    }
}