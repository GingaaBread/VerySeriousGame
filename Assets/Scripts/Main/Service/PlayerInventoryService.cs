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
        public event Action<ItemSo, int> OnConsumableUpdated;
        public event Action<int> OnCurrencyUpdated;
        public event Action<int, int> OnInventoryCapUpdated;

        public (int, int) GetCurrentCap() =>
            (_playerInventory.CurrentInventorySize, _playerInventory.CurrentInventoryLimit);

        public void IncrementCarryLimit()
        {
            _playerInventory.CurrentInventoryLimit++;
            OnInventoryCapUpdated?.Invoke(_playerInventory.CurrentInventorySize,
                _playerInventory.CurrentInventoryLimit);
            Debug.Log($"Increased the carry limit. It is now: {_playerInventory.CurrentInventoryLimit}");
        }

        public void Collect(ItemSo item, int amount = 1)
        {
            if (InventoryIsFull())
                Debug.LogWarning("Should not try to collect an item if the inventory is full. Check first!");

            if (!_playerInventory.ItemsInInventory.TryAdd(item, amount))
                _playerInventory.ItemsInInventory[item] += amount;

            Debug.Log($"Now the player has {_playerInventory.ItemsInInventory[item]}x {item.ItemName}");

            if (item == _currency)
            {
                OnCurrencyUpdated?.Invoke(_playerInventory.ItemsInInventory[item]);
                Debug.Log("Updating currency");
            }
            else
            {
                _playerInventory.CurrentInventorySize += amount;
                OnInventoryCapUpdated?.Invoke(_playerInventory.CurrentInventorySize,
                    _playerInventory.CurrentInventoryLimit);
            }

            if (item.IsConsumable) OnConsumableUpdated?.Invoke(item, _playerInventory.ItemsInInventory[item]);

            OnItemCollected?.Invoke(item);
        }

        public int MaximumOfRequired(ItemSo item, int requiredAmount)
        {
            if (requiredAmount >= 0)
            {
                var max = _playerInventory.ItemsInInventory.TryGetValue(item, out var amountInInventory)
                    ? Mathf.Min(requiredAmount, amountInInventory)
                    : 0;

                Debug.Log($"Calculated the maximum: {max} for amount of {item.ItemName} and required {requiredAmount}");
                return max;
            }

            Debug.LogError("Can only require positive amounts");
            return 0;
        }

        public bool CanRemove(Dictionary<ItemSo, int> items)
        {
            foreach (var (item, amount) in items)
            {
                Debug.Log($"Checking if {item.ItemName} exists at least {amount} times");
                if (!CanRemove(item, amount)) return false;
            }

            return true;
        }

        public bool CanRemove(ItemSo item, int amount)
        {
            if (amount is 0)
            {
                Debug.Log($"Can remove {item.ItemName} because the amount is 0 and therefore immediately passed");
                return true;
            }

            if (amount < 0)
            {
                Debug.Log($"Cannot remove {item.ItemName} because the required amount {amount} is < 0");
                return false;
            }

            var exists = _playerInventory.ItemsInInventory.TryGetValue(item, out var currentAmount);
            Debug.Log($"Does {item.ItemName} exist in the inventory? -> {exists}. It has {currentAmount}");

            var has = currentAmount >= amount;
            Debug.Log($"currentAmount {currentAmount} >= amount {amount}? -> {has}");

            var both = exists && has;
            Debug.Log($"Now both. It exists in the inventory and the player has enough of it? -> {both}");
            return both;
        }

        public void Remove(Dictionary<ItemSo, int> items)
        {
            foreach (var (item, amount) in items)
            {
                Remove(item, amount);
            }
        }

        public void Remove(ItemSo item, int amount)
        {
            Debug.Log($"Remove called!\n{Environment.StackTrace}");
            if (!_playerInventory.ItemsInInventory.ContainsKey(item))
            {
                Debug.LogError(
                    $"Cannot remove item {item.ItemName} because it does not exist in the inventory! Check first!");
                return;
            }

            _playerInventory.ItemsInInventory[item] -= amount;
            Debug.Log($"Removed item {amount} times");
            if (item.IsConsumable) OnConsumableUpdated?.Invoke(item, _playerInventory.ItemsInInventory[item]);

            var newAmount = _playerInventory.ItemsInInventory[item];
            if (newAmount < 0) Debug.LogError("Removing resulted in a negative amount. Check first!");

            if (newAmount <= 0)
            {
                _playerInventory.ItemsInInventory.Remove(item);
                Debug.Log($"Completely removed {item.ItemName} from the inventory");
            }

            if (item == _currency)
            {
                OnCurrencyUpdated?.Invoke(_playerInventory.ItemsInInventory.GetValueOrDefault(item, 0));
            }
            else
            {
                _playerInventory.CurrentInventorySize -= amount;
                OnInventoryCapUpdated?.Invoke(_playerInventory.CurrentInventorySize,
                    _playerInventory.CurrentInventoryLimit);
            }
        }

        public int CurrentCurrency() => _playerInventory.ItemsInInventory.GetValueOrDefault(_currency, 0);

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
            Debug.Log($"Trying to sell all of {item.ItemName}. There are {amount}");
            var total = amount * item.MoneyWorth;
            Remove(item, amount);
            Collect(_currency, total);
        }

        public (ItemSo, int) GetConsumable()
        {
            var i = _playerInventory.ItemsInInventory.FirstOrDefault(e => e.Key.IsConsumable);

            return i.Key == null ? (null, 0) : (i.Key, i.Value);
        }
    }
}