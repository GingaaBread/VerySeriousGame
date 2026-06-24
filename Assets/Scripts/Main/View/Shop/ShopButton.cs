using System.Collections.Generic;
using Lean.Pool;
using Main.Entity;
using Main.Entity.Upgrade;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View.Shop
{
    public class ShopButton : MonoBehaviour
    {
        [Required] [SerializeField] private Image _iconImage;
        [Required] [SerializeField] private RequiredResourceView _requiredResourcePrefab;
        [Required] [SerializeField] private Transform _resourceContainer;

        private readonly List<RequiredResourceView> _requiredResourceViews = new();
        private ShopView _callback;
        private ShopkeeperStock.SoldItem _renderedSoldItem;
        private UpgradeSo _renderedUpgrade;

        public void Render(ShopView callback, UpgradeSo upgradeSo, Dictionary<ItemSo, int> cost)
        {
            _renderedUpgrade = upgradeSo;
            _renderedSoldItem = null;
            _callback = callback;

            _iconImage.sprite = upgradeSo.Icon;
            DespawnAll();

            foreach (var (item, amount) in cost)
            {
                var instance = LeanPool.Spawn(_requiredResourcePrefab, _resourceContainer);
                instance.Render(item.ItemSprite, amount + string.Empty, Color.white);
                _requiredResourceViews.Add(instance);
            }
        }

        public void Render(ShopView callback, ShopkeeperStock.SoldItem soldItem)
        {
            _renderedUpgrade = null;
            _renderedSoldItem = soldItem;
            _callback = callback;

            _iconImage.sprite = soldItem.Item.ItemSprite;
            DespawnAll();

            foreach (var (costItem, amount) in soldItem.Cost)
            {
                var instance = LeanPool.Spawn(_requiredResourcePrefab, _resourceContainer);
                instance.Render(costItem.ItemSprite, amount + string.Empty, Color.green);
                _requiredResourceViews.Add(instance);
            }
        }

        public void OnClick()
        {
            if (_renderedUpgrade != null) _callback.Select(_renderedUpgrade);
            else if (_renderedSoldItem != null) _callback.Select(_renderedSoldItem);
        }

        private void DespawnAll()
        {
            _requiredResourceViews.ForEach(v => LeanPool.Despawn(v));
            _requiredResourceViews.Clear();
        }
    }
}