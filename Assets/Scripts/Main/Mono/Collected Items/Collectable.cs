using System.Collections;
using Main.Entity;
using Main.Service;
using UnityEngine;
using UnityEngine.Events;
using Utility.Polish;
using VContainer;

namespace Main.Mono.Collected_Items
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private float _sturdiness = 50;
        [SerializeField] private ItemSo _itemYield;

        [SerializeField] private UnityEvent _onHit;
        [SerializeField] private UnityEvent _onDestruction;
        [Inject] private readonly DrillService _drillService;
        [Inject] private readonly PlayerInventoryService _playerInventoryService;
        [Inject] private readonly PlayerStatService _playerStatService;

        private float _currentSturdiness;
        private Coroutine _miningRoutine;

        private void Awake()
        {
            _currentSturdiness = _sturdiness;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            _miningRoutine ??= StartCoroutine(MineRoutine());
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;

            if (_miningRoutine == null) return;

            StopCoroutine(_miningRoutine);
            _miningRoutine = null;
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
            IndicatorManager.Instance.RequireAt(damage + string.Empty, transform.position + Vector3.up * 3);
            _onHit?.Invoke();
        }

        private void Collect()
        {
            Debug.Log($"Destroyed {name}. Now collecting the item yield.");
            _playerInventoryService.Collect(_itemYield);
            IndicatorManager.Instance.RequireAt("BAM!", transform.position + Vector3.up * 3);
            _onDestruction?.Invoke();
        }
    }
}