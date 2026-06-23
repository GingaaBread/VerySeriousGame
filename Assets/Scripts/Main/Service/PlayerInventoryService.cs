using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class PlayerInventoryService
    {
        private readonly PlayerInventory _playerInventory = new();

        public event Action<ItemSo> OnItemCollected;

        public void IncrementCarryLimit()
        {
            _playerInventory.CurrentInventoryLimit++;
            Debug.Log($"Increased the carry limit. It is now: {_playerInventory.CurrentInventoryLimit}");
        }

        public void Collect(ItemSo item)
        {
            if (InventoryIsFull())
                Debug.LogWarning("Should not try to collect an item if the inventory is full. Check first!");

            if (!_playerInventory.ItemsInInventory.TryAdd(item, 1)) _playerInventory.ItemsInInventory[item]++;
            _playerInventory.CurrentInventorySize++;

            OnItemCollected?.Invoke(item);
            Debug.Log($"Collected 1x {item.ItemName}");
        }

        public void Remove(Dictionary<ItemSo, int> items)
        {
            Debug.Log($"Removing multiple ({items.Count}) items");
            foreach (var (item, amount) in items)
            {
                Remove(item, amount);
            }
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

            _playerInventory.CurrentInventorySize -= amount;
        }

        public bool InventoryIsFull() =>
            _playerInventory.CurrentInventorySize >= _playerInventory.CurrentInventoryLimit;
    }
}