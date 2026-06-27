using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class PlayerStatService
    {
        private const float MIN_BURST_INTERVAL = 0.3f;
        private const float BURST_DECAY_RATE = 0.2f; // 20%
        private readonly PlayerStats _playerStats = new();

        public float CurrentMiningStrength() => _playerStats.MiningStrength;
        public float CurrentMiningBurstInterval() => _playerStats.MiningBurstInterval;

        public void ImproveMiningStrength()
        {
            _playerStats.MiningStrength++;
        }

        public void ImproveMiningBurstInterval()
        {
            var reduced = _playerStats.MiningBurstInterval * (1f - BURST_DECAY_RATE);
            _playerStats.MiningBurstInterval = Mathf.Max
            (
                Mathf.Round(reduced * 100f) / 100f,
                MIN_BURST_INTERVAL
            );
        }
    }
}