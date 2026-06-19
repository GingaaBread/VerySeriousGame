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
        private readonly DrillStatus _drillStatus = new();

        public void Dispose()
        {
            _batteryService.OnBatteryEmptied -= DisallowActivation;
        }

        public void Start()
        {
            _batteryService.OnBatteryEmptied += DisallowActivation;
        }

        public event Action<bool> OnActivationChange;

        public void ToggleActivity()
        {
            if (!_drillStatus.CanActivate)
            {
                Debug.Log("Cannot activate right now.");
                return;
            }

            _drillStatus.IsActivated = !_drillStatus.IsActivated;
            OnActivationChange?.Invoke(_drillStatus.IsActivated);
            Debug.Log($"Toggled the drill. Is it now active? : {_drillStatus.IsActivated}");
        }

        public bool IsActivated() => _drillStatus.IsActivated;

        private void DisallowActivation()
        {
            _drillStatus.IsActivated = false;
            _drillStatus.CanActivate = false; 
        }
    }
}