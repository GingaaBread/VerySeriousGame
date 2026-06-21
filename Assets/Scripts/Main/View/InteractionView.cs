using Main.Service;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using VContainer;

namespace Main.View
{
    public class InteractionView : MonoBehaviour
    {
        private const string KEY = "E";
        [Required] [SerializeField] private GameObject _container;
        [Required] [SerializeField] private TMP_Text _keyText;
        [Required] [SerializeField] private TMP_Text _promptText;

        [Inject] private readonly InteractionService _interactionService;

        private void OnEnable()
        {
            _interactionService.OnInteractionsUpdated += Render;
        }

        private void OnDisable()
        {
            _interactionService.OnInteractionsUpdated -= Render;
        }

        private void Render(string prompt)
        {
            _container.SetActive(true);

            _keyText.text = KEY;
            _promptText.text = prompt;
        }

        private void Render(string[] prompts)
        {
            // Only renders the first interaction right now
            if (prompts.Length is 0) Hide();
            else Render(prompts[0]);
        }

        private void Hide()
        {
            _container.SetActive(false);
        }
    }
}