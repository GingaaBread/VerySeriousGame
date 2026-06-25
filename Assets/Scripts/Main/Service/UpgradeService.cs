using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using VContainer.Unity;

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class UpgradeService : IAsyncStartable
    {
        [Inject] private readonly PlayerInventoryService _playerInventoryService;
        private readonly UpgradeStatus _upgradeStatus = new();

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            var upgradePool = await Addressables.LoadAssetsAsync<UpgradeSo>("upgrade");
            Register(upgradePool);
        }

        public void Purchase(UpgradeSo upgrade)
        {
            if (!_playerInventoryService.CanRemove(upgrade.Cost))
            {
                Debug.Log("Ignoring the purchase request because the cost cannot be afforded");
                Audio.AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreError);
                return;
            }

            if (!_upgradeStatus.PurchasedUpgrades.TryAdd(upgrade, 1)) _upgradeStatus.PurchasedUpgrades[upgrade]++;

            _playerInventoryService.Remove(upgrade.Cost);
            Audio.AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreBuy);
            Debug.Log($"Purchased upgrade '{upgrade}'");
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
    }
}