using System;
using System.Collections.Generic;
using System.Linq;
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
            [field: SerializeField] public SerializedDictionary<ItemSo, int> PrivateCost { get; private set; }

            public Dictionary<ItemSo, int> Cost => PrivateCost.ToDictionary
                (kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}