using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Main.Mono.Interactions;
using UnityEngine;

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class InteractionService
    {
        private readonly List<InteractionPoint> _availableInteractions = new();
        public event Action<string[]> OnInteractionsUpdated;

        public void Trigger(int index)
        {
            if (index < 0 || index >= _availableInteractions.Count) return;

            var interaction = _availableInteractions[index];
            interaction.OnInteractionTriggered?.Invoke();

            if (!interaction.UnregisterOnInteraction) return;
            Unregister(interaction);
        }

        public void Register(InteractionPoint interactionPoint)
        {
            if (_availableInteractions.Contains(interactionPoint))
            {
                Debug.LogError("Cannot register already-registered interaction");
                return;
            }

            _availableInteractions.Add(interactionPoint);
            TriggerUpdateEvent();
        }

        private void TriggerUpdateEvent()
        {
            OnInteractionsUpdated?.Invoke(_availableInteractions.Select(i => i.PromptText).ToArray());
        }

        public void Unregister(InteractionPoint interactionPoint)
        {
            if (!_availableInteractions.Contains(interactionPoint)) return;

            _availableInteractions.Remove(interactionPoint);
            TriggerUpdateEvent();
        }
    }
}