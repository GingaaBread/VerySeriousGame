using System;
using JetBrains.Annotations;
using Main.Entity;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class DrillService : IStartable, IDisposable
    {
        [Inject] private readonly BatteryService _batteryService;
        [Inject] private readonly DrillActivationMediator _drillActivationMediator;
        private readonly DrillStatus _drillStatus = new();

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
            Debug.Log($"Toggled the drill. Is it now active? : {_drillStatus.IsActivated}");
        }

        public bool IsActivated() => _drillStatus.IsActivated;

        private void StopActivation()
        {
            _drillStatus.IsActivated = false;
        }
    }
}