using System.Collections.Generic;

namespace Main.Entity
{
    public class PlayerInventory
    {
        public Dictionary<ItemSo, int> ItemsInInventory { get; private set; } = new();
        public int CurrentInventoryLimit { get; set; } = 2;
        public int CurrentInventorySize { get; set; } = 0; // for easier access
    }
}