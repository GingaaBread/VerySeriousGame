using Main.Service;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace Main.Mono.Interactions
{
    public class InteractionPoint : MonoBehaviour
    {
        [field: SerializeField] public string PromptText { private set; get; } = "Interaction";
        [field: SerializeField] public bool UnregisterOnInteraction { private set; get; }
        [field: SerializeField] public UnityEvent OnInteractionTriggered { private set; get; }

        [Inject] private readonly InteractionService _interactionService;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _interactionService.Register(this);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            _interactionService.Unregister(this);
        }
    }
}