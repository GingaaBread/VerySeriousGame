using System.Collections;
using Main.Entity;
using Main.Service;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Main.View
{
    public class ItemUseView : MonoBehaviour
    {
        [Required] [SerializeField] private Image _itemImage;
        [Required] [SerializeField] private TMP_Text _amountText;

        [Inject] private PlayerInventoryService _playerInventoryService;

        private void OnEnable()
        {
            StartCoroutine(WaitForService());
        }

        private void OnDisable()
        {
            _playerInventoryService.OnConsumableUpdated -= UpdateCosumable;
        }

        private IEnumerator WaitForService()
        {
            yield return new WaitUntil(() => _playerInventoryService != null);
            _playerInventoryService.OnConsumableUpdated += UpdateCosumable;
            var consumable = _playerInventoryService.GetConsumable();
            UpdateCosumable(consumable.Item1, consumable.Item2);
        }

        private void UpdateCosumable(ItemSo item, int amount)
        {
            if (item == null || amount == 0) Hide();
            else Render(item.ItemSprite, amount);
        }

        private void Render(Sprite sprite, int amount)
        {
            _amountText.text = amount + string.Empty;
            _itemImage.gameObject.SetActive(true);
            _itemImage.sprite = sprite;
        }

        private void Hide()
        {
            _amountText.text = string.Empty;
            _itemImage.gameObject.SetActive(false);
        }
    }
}