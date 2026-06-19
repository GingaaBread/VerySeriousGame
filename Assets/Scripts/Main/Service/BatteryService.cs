using System;
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
        [Inject] private readonly DrillService _drillService;

        public void Dispose()
        {
            _drillService.OnActivationChange -= UpdateBatteryDrain;
        }

        public void Start()
        {
            _drillService.OnActivationChange += UpdateBatteryDrain;
        }

        private void UpdateBatteryDrain(bool drillIsActive)
        {
            throw new NotImplementedException();
        }

        public void IncreaseMaxBattery()
        {
            _batteryStatus.CurrentMaxBatteryLevel += MAX_BATTERY_INCREMENT;
            Debug.Log($"Increased the max battery. It is now: {_batteryStatus.CurrentMaxBatteryLevel}");
        }

        public void RechargeCompletely()
        {
            _batteryStatus.CurrentBatteryLevel = _batteryStatus.CurrentMaxBatteryLevel;
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

            Debug.Log($"Drained the battery level. It is now {_batteryStatus.CurrentBatteryLevel}");
        }

        public event Action OnBatteryEmptied;

        public bool BatteryIsEmpty() => _batteryStatus.CurrentBatteryLevel is 0;
    }
}