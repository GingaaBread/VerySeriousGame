using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Main.Entity;
using Main.Service;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;
using VContainer;

namespace Main.View.Shop
{
    public class ShopView : MonoBehaviour
    {
        [Required] [SerializeField] private GameObject _container;
        [Required] [SerializeField] private ShopButton _shopButtonPrefab;
        [Required] [SerializeField] private Transform _scrollViewContent;
        [Required] [SerializeField] private ShopDetailPage _shopDetailPage;
        [Required] [SerializeField] private GameObject _purchaseButton;
        [SerializeField] private UnityEvent _onClose;

        private readonly List<ShopButton> _instances = new();
        [Inject] private readonly PlayerInventoryService _playerInventoryService;
        [Inject] private readonly UpgradeService _upgradeService;
        private UpgradeSo _currentlySelected;

        private List<UpgradeSo> _upgrades;

        public void Select(UpgradeSo upgrade)
        {
            _currentlySelected = _upgrades.FirstOrDefault(e => e == upgrade);
            if (_currentlySelected == null) return;

            _purchaseButton.SetActive(_playerInventoryService.CanRemove(upgrade.Cost));

            _shopDetailPage.Render(upgrade);
        }

        public void TryPurchaseCurrent()
        {
            if (_currentlySelected == null) return;
            _upgradeService.Purchase(_currentlySelected);
            Select(_currentlySelected); // <- could be unavailable for purchase now
        }

        public void RenderUpgrades()
        {
            _upgrades = _upgradeService.GetAll().ToList();
            Render();
            Select(_upgrades[0]);
        }

        public void Close()
        {
            InputManager.Instance.PopUI();

            _container.SetActive(false);
            _onClose?.Invoke();
        }

        private void Render()
        {
            InputManager.Instance.PushUI();
            _container.SetActive(true);
            DespawnAll();
            foreach (var upgradeSo in _upgrades)
            {
                var instance = LeanPool.Spawn(_shopButtonPrefab, _scrollViewContent);
                _instances.Add(instance);
                instance.Render(this, upgradeSo, upgradeSo.Cost);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollViewContent as RectTransform);
                Debug.Log($"Rendering {upgradeSo.UpgradeName}");
            }
        }

        private void DespawnAll()
        {
            _instances.ForEach(i => LeanPool.Despawn(i));
            _instances.Clear();
        }
    }
}