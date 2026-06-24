using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using UnityEngine;

namespace Main.Entity.Upgrade
{
    [CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade", order = 0)]
    public class UpgradeSo : ScriptableObject
    {
        [field: SerializeField] public string UpgradeName { get; private set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; private set; }

        [field: Required]
        [field: SerializeField]
        [field: ShowAssetPreview]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        [field: HorizontalLine]
        public SerializedDictionary<ItemSo, int> Cost { get; private set; } = new();

        [field: SerializeField]
        public UpgradeEffect[] Effects { get; private set; }
    }
}