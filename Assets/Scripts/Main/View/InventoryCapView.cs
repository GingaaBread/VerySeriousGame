using Main.Service;
using TMPro;
using UnityEngine;
using VContainer;

namespace Main.View
{
    public class InventoryCapView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;

        [Inject] private readonly PlayerInventoryService _playerInventoryService;

        private void OnEnable()
        {
            _playerInventoryService.OnInventoryCapUpdated += Render;
            var cap = _playerInventoryService.GetCurrentCap();
            Render(cap.Item1, cap.Item2);
        }

        private void OnDisable()
        {
            _playerInventoryService.OnInventoryCapUpdated += Render;
        }

        private void Render(int heldAmount, int maxAmount)
        {
            _amountText.text = heldAmount + "/" + maxAmount;
        }
    }
}