using System.Collections.Generic;
using System.Linq;
using Audio;
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
        [Required] [SerializeField] private ItemSo _moneyItem;
        [Required] [SerializeField] private GameObject _container;
        [Required] [SerializeField] private ShopButton _shopButtonPrefab;
        [Required] [SerializeField] private Transform _scrollViewContent;
        [Required] [SerializeField] private ShopDetailPage _shopDetailPage;
        [Required] [SerializeField] private GameObject _purchaseButton;
        [Required] [SerializeField] private GameObject _buyButton;
        [Required] [SerializeField] private GameObject _sellButton;
        [Required] [SerializeField] private RequiredResourceView _requiredResourcePrefab;
        [Required] [SerializeField] private Transform _requiredResourceTransform;
        [SerializeField] private UnityEvent _onClose;
        private readonly List<RequiredResourceView> _detailResourceInstances = new();
        [Inject] private readonly PlayerInventoryService _playerInventoryService;
        private readonly List<ShopButton> _shopButtonInstances = new();
        [Inject] private readonly UpgradeService _upgradeService;
        private ShopkeeperStock.SoldItem _currentlySelectedItem;
        private ItemSo _currentlySelectedItemToSell;
        private UpgradeSo _currentlySelectedUpgrade;
        private bool _isShopOpen;
        private ShopkeeperStock _shopkeeperStock;
        private List<UpgradeSo> _upgrades;

        public void TryBuyCurrent()
        {
            if (_currentlySelectedItem == null || _playerInventoryService.InventoryIsFull() ||
                !_playerInventoryService.CanRemove(_currentlySelectedItem.Cost))
            {
                AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreError);
                return;
            }

            if (_currentlySelectedItem == null || _playerInventoryService.InventoryIsFull() ||
                !_playerInventoryService.CanRemove(_currentlySelectedItem.Cost)) return;
            _playerInventoryService.Collect(_currentlySelectedItem.Item);
            _playerInventoryService.Remove(_currentlySelectedItem.Cost);
            AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreBuy);
            Select(_currentlySelectedItem); // <- could be unavailable  now
        }

        public void TryPurchaseCurrent()
        {
            if (_currentlySelectedUpgrade == null) return;
            _upgradeService.Purchase(_currentlySelectedUpgrade);

            AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreBuy);
            Select(_currentlySelectedUpgrade); // <- could be unavailable for purchase now
        }

        public void TrySellCurrent()
        {
            if (_currentlySelectedItemToSell == null) return;
            _playerInventoryService.SellAllOf(_currentlySelectedItemToSell);
            RenderSell(); // <- is unavailable for purchase now
        }

        public void Select(UpgradeSo upgrade)
        {
            if (_currentlySelectedUpgrade != upgrade)
                AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreTab);

            _currentlySelectedUpgrade = _upgrades.FirstOrDefault(e => e == upgrade);
            _detailResourceInstances.ForEach(i => LeanPool.Despawn(i));
            _detailResourceInstances.Clear();
            _buyButton.SetActive(false);
            _sellButton.SetActive(false);

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

        public void Select(ItemSo renderedItemSoldByPlayer, ItemSo renderedItemGivenToPlayer,
            int renderedGivenAmount)
        {
            AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreTab);

            _currentlySelectedItemToSell = renderedItemSoldByPlayer;
            _detailResourceInstances.ForEach(i => LeanPool.Despawn(i));
            _detailResourceInstances.Clear();

            _sellButton.SetActive(true);
            _buyButton.SetActive(false);
            _purchaseButton.SetActive(false);

            var instance =
                LeanPool.Spawn(_requiredResourcePrefab, _requiredResourceTransform);
            instance.Render(renderedItemGivenToPlayer.ItemSprite, renderedGivenAmount + string.Empty, Color.green);
            _detailResourceInstances.Add(instance);

            _shopDetailPage.Render(renderedItemSoldByPlayer.ItemName, renderedItemSoldByPlayer.ItemDescription,
                renderedItemSoldByPlayer.ItemSprite);
        }

        public void Select(ShopkeeperStock.SoldItem soldItem)
        {
            if (_currentlySelectedItem != soldItem)
                AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreTab);
            _currentlySelectedItem = _shopkeeperStock.SoldItems.FirstOrDefault(e => e == soldItem);
            _detailResourceInstances.ForEach(i => LeanPool.Despawn(i));
            _detailResourceInstances.Clear();
            _purchaseButton.SetActive(false);
            _sellButton.SetActive(false);

            if (_currentlySelectedUpgrade == null) return;

            if (_playerInventoryService.InventoryIsFull())
            {
                _buyButton.SetActive(false);
                var instance =
                    LeanPool.Spawn(_requiredResourcePrefab, _requiredResourceTransform);
                instance.Render("Inventory full!", Color.red);
                _detailResourceInstances.Add(instance);
            }
            else if (_playerInventoryService.CanRemove(soldItem.Cost))
            {
                _buyButton.SetActive(true);
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

                _buyButton.SetActive(false);
            }

            _shopDetailPage.Render(soldItem.Item.ItemName, soldItem.Item.ItemDescription, soldItem.Item.ItemSprite);
        }

        public void DeselectAll()
        {
            foreach (var shopButtonInstance in _shopButtonInstances)
            {
                shopButtonInstance.Deselect();
            }
        }

        public void RenderSell()
        {
            var inventory = _playerInventoryService.GetAll();
            Render();
            foreach (var (item, amount) in inventory)
            {
                var instance = LeanPool.Spawn(_shopButtonPrefab, _scrollViewContent);
                _shopButtonInstances.Add(instance);
                instance.Render(this, item, amount, _moneyItem, item.MoneyWorth);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollViewContent as RectTransform);
            }

            if (inventory.Count is 0)
            {
                DespawnAll();
                _shopDetailPage.HideContent();
                return;
            }

            var any = inventory.First();
            Select(any.Key, _moneyItem, any.Key.MoneyWorth);
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

            if (_upgrades.Count is not 0) Select(_upgrades[0]);
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

            if (_upgrades.Count is not 0) Select(_shopkeeperStock.SoldItems[0]);
        }

        public void Close()
        {
            InputManager.Instance.PopUI();

            _container.SetActive(false);
            _onClose?.Invoke();
            _isShopOpen = false;
        }

        private void Render()
        {
            InputManager.Instance.PushUI();
            _container.SetActive(true);
            DespawnAll();
            if (!_isShopOpen)
            {
                AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreOpen);
                _isShopOpen = true;
            }
            else
            {
                AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreTab);
            }
        }

        private void DespawnAll()
        {
            _shopButtonInstances.ForEach(i => LeanPool.Despawn(i));
            _shopButtonInstances.Clear();
        }
    }
}