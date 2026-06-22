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
    public sealed class UpgradeService : IAsyncStartable
    {
        private readonly UpgradeStatus _upgradeStatus = new();

        public async UniTask StartAsync(CancellationToken cancellation)
        {
            var upgradePool = await Addressables.LoadAssetsAsync<UpgradeSo>("upgrade");
            Register(upgradePool);
        }

        public void Purchase(UpgradeSo upgrade)
        {
            if (!_upgradeStatus.PurchasedUpgrades.TryAdd(upgrade, 1)) _upgradeStatus.PurchasedUpgrades[upgrade]++;

            Debug.Log($"Purchased upgrade '{upgrade}'");
        }

        private void Register(IEnumerable<UpgradeSo> upgrades)
        {
            var test = upgrades.ToList();
            Debug.Log($"Trying to register {test.Count()} upgrades");
            if (_upgradeStatus.UpgradePool.Count is not 0)
            {
                Debug.LogWarning("Should not register upgrades because some have already been registered before");
                return;
            }

            _upgradeStatus.UpgradePool = test.ToList();
        }

        public UpgradeSo[] GetAll()
        {
            var all = _upgradeStatus.UpgradePool;
            return all.ToArray();
        }
    }
}