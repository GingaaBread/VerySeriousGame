using System;
using System.Collections.Generic;

namespace Utility.Audio
{
    public static class ShuffleService
    {
        private static readonly Random _random = new();

        public static void Shuffle<T>(IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException(nameof(list));

            for (var i = list.Count - 1; i > 0; i--)
            {
                var j = _random.Next(i + 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }
    }
}