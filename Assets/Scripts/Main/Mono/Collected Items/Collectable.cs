using System.Collections;
using Audio;
using Lean.Pool;
using Main.Entity;
using Main.Mono.Serialisation;
using Main.Service;
using Main.View;
using UnityEngine;
using UnityEngine.Events;
using Utility.Polish;
using VContainer;

namespace Main.Mono.Collected_Items
{
    /// <summary>
    ///     Important: the UniqueGameId disallows pooling
    /// </summary>
    [RequireComponent(typeof(UniqueGameId))]
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private float _sturdiness = 50;
        [SerializeField] private ItemSo _itemYield;

        [SerializeField] private UnityEvent _onHit;
        [SerializeField] private UnityEvent _onDestruction;
        [Inject] private readonly DrillService _drillService;
        [Inject] private readonly PlayerInventoryService _playerInventoryService;
        [Inject] private readonly PlayerStatService _playerStatService;
        [Inject] private readonly SerialisationService _serialisationService;
        private float _currentSturdiness;
        private HealthBarView _healthBar;

        private UniqueGameId _id;
        private float _initialSturdiness;
        private Coroutine _miningRoutine;

        private void Awake()
        {
            _id = GetComponent<UniqueGameId>();
            _currentSturdiness = _sturdiness;
            _initialSturdiness = _sturdiness;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            _miningRoutine ??= StartCoroutine(MineRoutine());
            CreateHealthBarIfRequired();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            ClearHealthBar();

            if (_miningRoutine == null) return;

            StopCoroutine(_miningRoutine);
            _miningRoutine = null;
        }

        private void CreateHealthBarIfRequired()
        {
            if (_healthBar != null) return;
            _healthBar =
                HealthBarManager.Instance.SpawnNewAt(transform.position + Vector3.down, GetCurrentPercentage());
        }

        private float GetCurrentPercentage() => _currentSturdiness / _initialSturdiness;

        private void ClearHealthBar()
        {
            if (_healthBar == null) return;
            LeanPool.Despawn(_healthBar);
            _healthBar = null;
        }

        private IEnumerator MineRoutine()
        {
            while (_currentSturdiness > 0)
            {
                yield return new WaitForSeconds(_playerStatService.CurrentMiningBurstInterval());

                if (!_drillService.IsActivated())
                {
                    Debug.Log("Cannot mine because the drill is not active :)");
                    continue;
                }

                if (_playerInventoryService.InventoryIsFull())
                {
                    Debug.Log("Cannot mine because the inventory is full :)");
                    // its okay to break here because the inventory cannot be emptied from within the radius
                    yield break;
                }

                Hit();

                if (_currentSturdiness > 0) continue;

                Collect();
                yield break;
            }
        }

        private void Hit()
        {
            var damage = _playerStatService.CurrentMiningStrength();
            _currentSturdiness -= damage;

            AudioManager.Instance.PlayOneShot(AudioRegistry.Events.DrillHitResource);

            IndicatorManager.Instance.RequireAt(damage + string.Empty, transform.position + Vector3.up * 3);
            if (_healthBar != null) _healthBar.UpdatePercentage(GetCurrentPercentage());

            _onHit?.Invoke();
        }

        private void Collect()
        {
            Debug.Log($"Destroyed {name}. Now collecting the item yield.");
            _playerInventoryService.Collect(_itemYield);
            _serialisationService.MarkAsDestroyed(_id.Identifier);

            AudioManager.Instance.PlayOneShot3D(AudioRegistry.Events.DrillDestroyResource, transform.position);

            IndicatorManager.Instance.RequireAt("BAM!", transform.position + Vector3.up * 3);
            ClearHealthBar();

            _onDestruction?.Invoke();
        }
    }
}