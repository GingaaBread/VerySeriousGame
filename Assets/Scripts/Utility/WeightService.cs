using System;
using System.Linq;
using Random = UnityEngine.Random;

namespace Utility
{
    public static class WeightService
    {
        public static T Draw<T>(T[] items, int[] weights)
        {
            if (items.Length != weights.Length)
                throw new ArgumentException("Items and weights must be the same length.");

            var totalWeight = weights.Sum();
            var roll = Random.Range(0, totalWeight);

            var cumulative = 0;
            for (var i = 0; i < items.Length; i++)
            {
                cumulative += weights[i];
                if (roll < cumulative)
                    return items[i];
            }

            return items[^1];
        }
    }
}