using System;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Main.Entity
{
    public class ShopkeeperStock : MonoBehaviour
    {
        [field: SerializeField] public SoldItem[] SoldItems { get; private set; }

        [Serializable]
        public class SoldItem
        {
            [field: SerializeField] public ItemSo Item { get; private set; }
            [field: SerializeField] public SerializedDictionary<ItemSo, int> Cost { get; private set; }
        }
    }
}