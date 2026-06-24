using System.Collections.Generic;
using System.Linq;
using Lean.Pool;
using Main.Entity;
using Main.Entity.Upgrade;
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
        [Required] [SerializeField] private RequiredResourceView _requiredResourcePrefab;
        [Required] [SerializeField] private Transform _requiredResourceTransform;
        [SerializeField] private UnityEvent _onClose;
        private readonly List<RequiredResourceView> _detailResourceInstances = new();
        [Inject] private readonly PlayerInventoryService _playerInventoryService;
        private readonly List<ShopButton> _shopButtonInstances = new();
        [Inject] private readonly UpgradeService _upgradeService;
        private ShopkeeperStock.SoldItem _currentlySelectedItem;
        private UpgradeSo _currentlySelectedUpgrade;
        private ShopkeeperStock _shopkeeperStock;
        private List<UpgradeSo> _upgrades;

        public void Select(UpgradeSo upgrade)
        {
            _currentlySelectedUpgrade = _upgrades.FirstOrDefault(e => e == upgrade);
            _detailResourceInstances.ForEach(i => LeanPool.Despawn(i));
            _detailResourceInstances.Clear();

            if (_currentlySelectedUpgrade == null) return;

            if (_playerInventoryService.CanRemove(upgrade.Cost))
            {
                _purchaseButton.SetActive(true);
            }
            else
            {
                foreach (var (item, requiredAmount) in upgrade.Cost)
                {
                    var maxAvailable = _playerInventoryService.MaximumOfRequired(item, requiredAmount);
                    if (maxAvailable >= requiredAmount) continue;

                    var instance =
                        LeanPool.Spawn(_requiredResourcePrefab, _requiredResourceTransform);
                    instance.Render(item.ItemSprite, maxAvailable + "/" + requiredAmount, Color.red);
                    _detailResourceInstances.Add(instance);
                }

                _purchaseButton.SetActive(false);
            }

            _shopDetailPage.Render(upgrade.UpgradeName, upgrade.Description, upgrade.Icon);
        }

        public void Select(ShopkeeperStock.SoldItem soldItem)
        {
            _currentlySelectedItem = _shopkeeperStock.SoldItems.FirstOrDefault(e => e == soldItem);
            _detailResourceInstances.ForEach(i => LeanPool.Despawn(i));
            _detailResourceInstances.Clear();

            if (_currentlySelectedUpgrade == null) return;

            if (_playerInventoryService.CanRemove(soldItem.Cost))
            {
                _purchaseButton.SetActive(true);
            }
            else
            {
                foreach (var (item, requiredAmount) in soldItem.Cost)
                {
                    var maxAvailable = _playerInventoryService.MaximumOfRequired(item, requiredAmount);
                    if (maxAvailable >= requiredAmount) continue;

                    var instance =
                        LeanPool.Spawn(_requiredResourcePrefab, _requiredResourceTransform);
                    instance.Render(item.ItemSprite, maxAvailable + "/" + requiredAmount, Color.red);
                    _detailResourceInstances.Add(instance);
                }

                _purchaseButton.SetActive(false);
            }

            _shopDetailPage.Render(soldItem.Item.ItemName, soldItem.Item.ItemDescription, soldItem.Item.ItemSprite);
        }

        public void TryPurchaseCurrent()
        {
            if (_currentlySelectedUpgrade == null) return;
            _upgradeService.Purchase(_currentlySelectedUpgrade);
            Select(_currentlySelectedUpgrade); // <- could be unavailable for purchase now
        }

        public void RenderUpgrades()
        {
            _upgrades = _upgradeService.GetAll().ToList();
            Render();
            foreach (var upgradeSo in _upgrades)
            {
                var instance = LeanPool.Spawn(_shopButtonPrefab, _scrollViewContent);
                _shopButtonInstances.Add(instance);
                instance.Render(this, upgradeSo, upgradeSo.Cost);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollViewContent as RectTransform);
            }

            Select(_upgrades[0]);
        }

        public void Render(ShopkeeperStock shopkeeperStock)
        {
            _shopkeeperStock = shopkeeperStock;
            RenderUpgrades();
        }

        public void RenderSoldItems()
        {
            Render();
            foreach (var item in _shopkeeperStock.SoldItems)
            {
                var instance = LeanPool.Spawn(_shopButtonPrefab, _scrollViewContent);
                _shopButtonInstances.Add(instance);
                instance.Render(this, item);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollViewContent as RectTransform);
            }

            Select(_shopkeeperStock.SoldItems[0]);
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
        }

        private void DespawnAll()
        {
            _shopButtonInstances.ForEach(i => LeanPool.Despawn(i));
            _shopButtonInstances.Clear();
        }
    }
}