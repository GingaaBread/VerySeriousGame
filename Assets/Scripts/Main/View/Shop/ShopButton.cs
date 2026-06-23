using System.Collections.Generic;
using Lean.Pool;
using Main.Entity;
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
        private UpgradeSo _renderedUpgrade;

        public void Render(ShopView callback, UpgradeSo upgradeSo, Dictionary<ItemSo, int> cost)
        {
            _renderedUpgrade = upgradeSo;
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

        public void OnClick()
        {
            _callback.Select(_renderedUpgrade);
        }

        private void DespawnAll()
        {
            _requiredResourceViews.ForEach(v => LeanPool.Despawn(v));
            _requiredResourceViews.Clear();
        }
    }
}