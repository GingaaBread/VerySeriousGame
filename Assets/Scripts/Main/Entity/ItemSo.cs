using NaughtyAttributes;
using UnityEngine;

namespace Main.Entity
{
    [CreateAssetMenu(fileName = "Item", menuName = "Item", order = 0)]
    public class ItemSo : ScriptableObject
    {
        [field: SerializeField] public string ItemName { get; private set; } = "New Item";
        [field: SerializeField] [field: TextArea] public string ItemDescription { get; private set; } = "New Item";

        [field: SerializeField]
        [field: ShowAssetPreview]
        public Sprite ItemSprite { get; private set; }

        [field: SerializeField] public int MoneyWorth { get; private set; }
        [field: SerializeField] public bool IsConsumable { get; private set; }
    }
}