using Main.Service;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using VContainer;

namespace Main.View.Shop
{
    public class CurrencyView : MonoBehaviour
    {
        [Required] [SerializeField] private TMP_Text _currencyText;

        [Inject] private readonly PlayerInventoryService _playerInventoryService;

        private void OnEnable()
        {
            _playerInventoryService.OnCurrencyUpdated += Render;
            var currency = _playerInventoryService.CurrentCurrency();
            Render(currency);
        }

        private void OnDisable()
        {
            _playerInventoryService.OnCurrencyUpdated -= Render;
        }

        private void Render(int amount)
        {
            _currencyText.text = string.Empty + amount;
        }
    }
}