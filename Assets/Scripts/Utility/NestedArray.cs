using System;
using UnityEngine;

namespace Utility
{
    [Serializable]
    public class NestedArray<T>
    {
        [field: SerializeField] public T[] Content { get; private set; }

        public NestedArray(T[] content)
        {
            Content = content;
        }
    }
}