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
            var service = ResolveService<PlayerStatService>();

            service?.ImproveMiningStrength();
        }

        [MenuItem("Tools/Testing/Improve Mining Burst Interval %#2")]
        private static void ImproveMiningBurstInterval()
        {
            var service = ResolveService<PlayerStatService>();

            service?.ImproveMiningBurstInterval();
        }

        [MenuItem("Tools/Testing/Increase Inventory Limit")]
        private static void IncreaseInventoryLimit()
        {
            var service = ResolveService<PlayerInventoryService>();

            service?.IncrementCarryLimit();
        }

        [MenuItem("Tools/Testing/Improve Mining Strength %#1", true)]
        [MenuItem("Tools/Testing/Improve Mining Burst Speed %#2", true)]
        private static bool ValidateRequiresPlayMode() => Application.isPlaying;

        private static T ResolveService<T>()
        {
            var scope = Object.FindFirstObjectByType<LifetimeScope>();
            if (scope != null) return scope.Container.Resolve<T>();
            Debug.LogWarning("No LifetimeScope found in scene.");
            return default;
        }
    }
}
#endif