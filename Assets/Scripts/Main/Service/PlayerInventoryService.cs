using System;
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

        public bool InventoryIsFull() =>
            _playerInventory.CurrentInventorySize >= _playerInventory.CurrentInventoryLimit;
    }
}