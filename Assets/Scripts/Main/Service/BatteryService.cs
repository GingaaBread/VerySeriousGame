using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using BatteryStatus = Main.Entity.BatteryStatus;

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class BatteryService : IStartable, IDisposable
    {
        private const int MAX_BATTERY_INCREMENT = 25;

        private readonly BatteryStatus _batteryStatus = new();
        [Inject] private readonly DrillActivationMediator _drillActivationMediator;

        private CancellationTokenSource _drainCts;

        public void Dispose()
        {
            _drillActivationMediator.OnActivationChange -= UpdateBatteryDrain;
            StopDraining();
        }

        public void Start()
        {
            _drillActivationMediator.OnActivationChange += UpdateBatteryDrain;
            TriggerAmountUpdate();
        }

        private void UpdateBatteryDrain(bool drillIsActive)
        {
            if (drillIsActive) StartDraining();
            else StopDraining();
        }

        private void StartDraining()
        {
            if (_drainCts != null) return;

            _drainCts = new();
            DrainLoop(_drainCts.Token).Forget();
        }

        private void StopDraining()
        {
            if (_drainCts == null) return;

            _drainCts.Cancel();
            _drainCts.Dispose();
            _drainCts = null;
        }

        private async UniTaskVoid DrainLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                DrainBattery();

                await UniTask.Delay(
                        TimeSpan.FromSeconds(_batteryStatus.CurrentDepletionInterval), cancellationToken: token)
                    .SuppressCancellationThrow();

                if (token.IsCancellationRequested) break;
                if (!BatteryIsEmpty()) continue;

                StopDraining();
                break;
            }
        }

        public void IncreaseMaxBattery()
        {
            _batteryStatus.CurrentMaxBatteryLevel += MAX_BATTERY_INCREMENT;
            TriggerAmountUpdate();
            Debug.Log($"Increased the max battery. It is now: {_batteryStatus.CurrentMaxBatteryLevel}");
        }

        public void IncreaseBatteryDepletionInterval()
        {
            _batteryStatus.CurrentDepletionInterval++;
            Debug.Log(
                $"Increased the battery depletion interval. It is now: {_batteryStatus.CurrentDepletionInterval}");
        }

        public void RechargeCompletely()
        {
            _batteryStatus.CurrentBatteryLevel = _batteryStatus.CurrentMaxBatteryLevel;
            TriggerAmountUpdate();
            Debug.Log($"Recharged the battery. It is now: {_batteryStatus.CurrentBatteryLevel}");
        }

        private void DrainBattery()
        {
            if (BatteryIsEmpty())
            {
                Debug.Log("Cannot drain the battery because it is empty. Check first!");
                return;
            }

            _batteryStatus.CurrentBatteryLevel--;
            if (_batteryStatus.CurrentBatteryLevel <= 0)
            {
                _batteryStatus.CurrentBatteryLevel = 0;
                OnBatteryEmptied?.Invoke();
            }

            TriggerAmountUpdate();
            Debug.Log($"Drained the battery level. It is now {_batteryStatus.CurrentBatteryLevel}");
        }

        private void TriggerAmountUpdate()
        {
            OnBatteryAmountUpdated?.Invoke(_batteryStatus.CurrentBatteryLevel, _batteryStatus.CurrentMaxBatteryLevel);
        }

        public event Action OnBatteryEmptied;
        public event Action<int, int> OnBatteryAmountUpdated;

        public bool BatteryIsEmpty() => _batteryStatus.CurrentBatteryLevel is 0;

        public int GetCurrentBattery() => _batteryStatus.CurrentBatteryLevel;
        public int GetCurrentMaxBattery() => _batteryStatus.CurrentMaxBatteryLevel;
    }
}