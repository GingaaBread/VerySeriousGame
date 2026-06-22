using System.Collections.Generic;
using Lean.Pool;
using Main.Entity;
using Main.Service;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Utility;
using VContainer;

namespace Main.View
{
    public class ShopView : MonoBehaviour
    {
        [Required] [SerializeField] private GameObject _container;
        [Required] [SerializeField] private ShopButton _shopButtonPrefab;
        [Required] [SerializeField] private Transform _scrollViewContent;
        [SerializeField] private UnityEvent _onClose;

        private readonly List<ShopButton> _instances = new();
        [Inject] private readonly UpgradeService _upgradeService;

        public void RenderUpgrades()
        {
            Render(_upgradeService.GetAll());
        }

        public void Close()
        {
            InputManager.Instance.PopUI();

            _container.SetActive(false);
            _onClose?.Invoke();
        }

        private void Render(UpgradeSo[] upgrades)
        {
            InputManager.Instance.PushUI();
            _container.SetActive(true);
            DespawnAll();
            foreach (var upgradeSo in upgrades)
            {
                var instance = LeanPool.Spawn(_shopButtonPrefab, _scrollViewContent);
                _instances.Add(instance);
                instance.Render(upgradeSo, upgradeSo.Cost);
                LayoutRebuilder.ForceRebuildLayoutImmediate(_scrollViewContent as RectTransform);
                Debug.Log($"Rendering {upgradeSo.UpgradeName}");
            }
        }

        private void DespawnAll()
        {
            _instances.ForEach(i => LeanPool.Despawn(i));
            _instances.Clear();
        }
    }
}