using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Main.Entity.Upgrade;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class UpgradeService : IAsyncStartable
    {
        [Inject] private readonly BatteryService _batteryService;
        [Inject] private readonly PlayerInventoryService _playerInventoryService;
        [Inject] private readonly PlayerStatService _playerStatService;
        private readonly UpgradeStatus _upgradeStatus = new();

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            var upgradePool = await Addressables.LoadAssetsAsync<UpgradeSo>("upgrade");
            Register(upgradePool);
        }

        public void Purchase(UpgradeSo upgrade)
        {
            var totalCost = upgrade.Cost.ToDictionary(
                item => item.Key,
                amount =>
                    amount.Value + upgrade.UpgradeIncreaseCost * UpgradeCountOf(upgrade)
            );

            if (!_playerInventoryService.CanRemove(totalCost))
            {
                Debug.Log("Ignoring the purchase request because the cost cannot be afforded");
                return;
            }

            if (!_upgradeStatus.PurchasedUpgrades.TryAdd(upgrade, 1)) _upgradeStatus.PurchasedUpgrades[upgrade]++;

            _playerInventoryService.Remove(totalCost);
            foreach (var upgradeEffect in upgrade.Effects)
            {
                ApplyUpgrade(upgradeEffect);
            }

            Debug.Log($"Purchased upgrade '{upgrade}'");
        }

        private void ApplyUpgrade(UpgradeEffect effect)
        {
            switch (effect)
            {
                case UpgradeEffect.BATTERY_CAP:
                    _batteryService.IncreaseMaxBattery();
                    break;
                case UpgradeEffect.BATTERY_INTERVAL:
                    _batteryService.IncreaseBatteryDepletionInterval();
                    break;
                case UpgradeEffect.INVENTORY_CAP:
                    _playerInventoryService.IncrementCarryLimit();
                    break;
                case UpgradeEffect.ATTACK_DAMAGE:
                    _playerStatService.ImproveMiningStrength();
                    break;
                case UpgradeEffect.ATTACK_INTERVAL:
                    _playerStatService.ImproveMiningBurstInterval();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(effect), effect, "Upgrade does not exist yet");
            }
        }

        private void Register(IEnumerable<UpgradeSo> upgrades)
        {
            if (_upgradeStatus.UpgradePool.Count is not 0)
            {
                Debug.LogWarning("Should not register upgrades because some have already been registered before");
                return;
            }

            _upgradeStatus.UpgradePool = upgrades.ToList();
        }

        public IEnumerable<UpgradeSo> GetAll()
        {
            var all = _upgradeStatus.UpgradePool;
            return all.ToArray();
        }

        public int UpgradeCountOf(UpgradeSo upgrade) => _upgradeStatus.PurchasedUpgrades.GetValueOrDefault(upgrade, 0);
    }
}