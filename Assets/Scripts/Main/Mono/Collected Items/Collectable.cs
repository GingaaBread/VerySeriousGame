using System.Collections;
using Main.Entity;
using Main.Service;
using UnityEngine;
using VContainer;

namespace Main.Mono.Collected_Items
{
    public class Collectable : MonoBehaviour
    {
        [SerializeField] private float _sturdiness = 50;
        [SerializeField] private ItemSo _itemYield;

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

                _currentSturdiness -= _playerStatService.CurrentMiningStrength();

                if (_currentSturdiness > 0) continue;

                Collect();
                yield break;
            }
        }

        private void Collect()
        {
            Debug.Log($"Collected {name}");
            Destroy(gameObject);
        }
    }
}