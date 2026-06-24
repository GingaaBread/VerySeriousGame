using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer.Unity;

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class PlayerInventoryService : IAsyncStartable
    {
        private readonly PlayerInventory _playerInventory = new();
        private ItemSo _currency;

        public async UniTask StartAsync(CancellationToken cancellation) =>
            _currency = await Addressables.LoadAssetAsync<ItemSo>("currency");

        public event Action<ItemSo> OnItemCollected;

        public void IncrementCarryLimit()
        {
            _playerInventory.CurrentInventoryLimit++;
            Debug.Log($"Increased the carry limit. It is now: {_playerInventory.CurrentInventoryLimit}");
        }

        public void Collect(ItemSo item, int amount = 1)
        {
            if (InventoryIsFull())
                Debug.LogWarning("Should not try to collect an item if the inventory is full. Check first!");

            if (!_playerInventory.ItemsInInventory.TryAdd(item, amount)) _playerInventory.ItemsInInventory[item]++;

            if (item != _currency) _playerInventory.CurrentInventorySize++;

            OnItemCollected?.Invoke(item);
            Debug.Log($"Collected 1x {item.ItemName}");
        }

        public void Remove(Dictionary<ItemSo, int> items)
        {
            foreach (var (item, amount) in items)
            {
                Remove(item, amount);
            }
        }

        public int MaximumOfRequired(ItemSo item, int required)
        {
            if (required >= 0)
                return !_playerInventory.ItemsInInventory.ContainsKey(item)
                    ? 0
                    : Mathf.Min(required, _playerInventory.ItemsInInventory[item]);

            Debug.LogError("Can only require positive amounts");
            return -1;
        }

        public bool CanRemove(Dictionary<ItemSo, int> items)
        {
            foreach (var (item, amount) in items)
            {
                if (!CanRemove(item, amount)) return false;
            }

            return true;
        }

        private bool CanRemove(ItemSo item, int amount)
        {
            if (amount < 0)
                return false;

            return _playerInventory.ItemsInInventory.TryGetValue(item, out var currentAmount)
                   && currentAmount >= amount;
        }

        private void Remove(ItemSo item, int amount)
        {
            if (!_playerInventory.ItemsInInventory.ContainsKey(item))
            {
                Debug.LogError(
                    $"Cannot remove item {item.ItemName} because it does not exist in the inventory! Check first!");
                return;
            }

            _playerInventory.ItemsInInventory[item] -= amount;

            var newAmount = _playerInventory.ItemsInInventory[item];
            if (newAmount < 0) Debug.LogError("Removing resulted in a negative amount. Check first!");

            if (newAmount <= 0)
            {
                _playerInventory.ItemsInInventory.Remove(item);
                Debug.Log($"Completely removed {item.ItemName} from the inventory");
            }

            if (item != _currency) _playerInventory.CurrentInventorySize -= amount;
        }

        public bool InventoryIsFull() =>
            _playerInventory.CurrentInventorySize >= _playerInventory.CurrentInventoryLimit;

        public Dictionary<ItemSo, int> GetAll() => _playerInventory
            .ItemsInInventory
            .Where(kvp => kvp.Key != _currency)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

        public void SellAllOf(ItemSo item)
        {
            if (!_playerInventory.ItemsInInventory.ContainsKey(item)) return;
            var amount = _playerInventory.ItemsInInventory[item];
            var total = amount * item.MoneyWorth;
            Remove(item, amount);
            Collect(_currency, total);
        }
    }
}