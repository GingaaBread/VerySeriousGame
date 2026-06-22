using System.Collections.Generic;

namespace Main.Entity
{
    public class UpgradeStatus
    {
        public List<UpgradeSo> UpgradePool { get; set; } = new();
        public Dictionary<UpgradeSo, int> PurchasedUpgrades { get; private set; } = new();
    }
}