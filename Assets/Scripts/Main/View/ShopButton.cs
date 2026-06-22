using System.Collections.Generic;
using Lean.Pool;
using Main.Entity;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace Main.View
{
    public class ShopButton : MonoBehaviour
    {
        [Required] [SerializeField] private Image _iconImage;
        [Required] [SerializeField] private RequiredResourceView _requiredResourcePrefab;
        [Required] [SerializeField] private Transform _resourceContainer;

        private readonly List<RequiredResourceView> _requiredResourceViews = new();

        public void Render(UpgradeSo upgradeSo, Dictionary<ItemSo, int> cost)
        {
            _iconImage.sprite = upgradeSo.Icon;
            DespawnAll();

            foreach (var (item, amount) in cost)
            {
                var instance = LeanPool.Spawn(_requiredResourcePrefab, _resourceContainer);
                instance.Render(item.ItemSprite, amount);
                _requiredResourceViews.Add(instance);
            }
        }

        private void DespawnAll()
        {
            _requiredResourceViews.ForEach(v => LeanPool.Despawn(v));
            _requiredResourceViews.Clear();
        }
    }
}