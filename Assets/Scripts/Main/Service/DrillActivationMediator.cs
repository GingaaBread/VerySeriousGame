using System;
using JetBrains.Annotations;

namespace Main.Service
{
    [UsedImplicitly]
    public sealed class DrillActivationMediator
    {
        public event Action<bool> OnActivationChange;
        public void RaiseActivationChange(bool isActive) => OnActivationChange?.Invoke(isActive);
    }
}