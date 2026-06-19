#if UNITY_EDITOR
using Main.Service;
using UnityEditor;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Main.Editor
{
    public static class TestingTools
    {
        [MenuItem("Tools/Testing/Improve Mining Strength %#1")]
        private static void ImproveMiningStrength()
        {
            var service = ResolveService();
            if (service == null) return;

            service.ImproveMiningStrength();
            Debug.Log($"Mining Strength: {service.CurrentMiningStrength()}");
        }

        [MenuItem("Tools/Testing/Improve Mining Burst Interval %#2")]
        private static void ImproveMiningBurstInterval()
        {
            var service = ResolveService();
            if (service == null) return;

            service.ImproveMiningBurstInterval();
            Debug.Log($"Mining Burst Interval: {service.CurrentMiningBurstInterval()}");
        }

        [MenuItem("Tools/Testing/Improve Mining Strength %#1", true)]
        [MenuItem("Tools/Testing/Improve Mining Burst Speed %#2", true)]
        private static bool ValidateRequiresPlayMode() => Application.isPlaying;

        private static PlayerStatService ResolveService()
        {
            var scope = Object.FindFirstObjectByType<LifetimeScope>();
            if (scope == null)
            {
                Debug.LogWarning("No LifetimeScope found in scene.");
                return null;
            }

            return scope.Container.Resolve<PlayerStatService>();
        }
    }
}
#endif