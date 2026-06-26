using System;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;
using VContainer;
using VContainer.Unity;

// add this

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class DrillService : IStartable, IDisposable
    {
        [Inject] private readonly BatteryService _batteryService;
        [Inject] private readonly DrillActivationMediator _drillActivationMediator;

        private readonly DrillStatus _drillStatus = new();
        private bool _batteryLossCooldown;

        public void Dispose()
        {
            _batteryService.OnBatteryEmptied -= StopActivation;
        }

        public void Start()
        {
            _batteryService.OnBatteryEmptied += StopActivation;
        }

        public void ToggleActivity()
        {
            if (_batteryService.BatteryIsEmpty())
            {
                Debug.Log("Cannot activate the drill because the battery is empty.");
                return;
            }

            _drillStatus.IsActivated = !_drillStatus.IsActivated;
            _drillActivationMediator.RaiseActivationChange(_drillStatus.IsActivated);

            StopIfEmpty();
        }

        public bool IsActivated() => _drillStatus.IsActivated;

        public void StopActivation()
        {
            Debug.Log("Stopping activation");
            _drillStatus.IsActivated = false;
            _drillActivationMediator.RaiseActivationChange(false);
        }

        private async void StopIfEmpty()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(0.3));
            Debug.Log("Stopping if empty");
            if (_batteryService.BatteryIsEmpty()) StopActivation();
        }
    }
}