using System;
using UnityEngine;

namespace Utility.Transitions
{
    [Serializable]
    public class StyleTimeline
    {
        [field: SerializeField]
        public TransitionStyle Style { get; private set; }

        [field: SerializeField]
        public LeanTweenType Ease { get; private set; }

        [field: SerializeField]
        [field: Min(0f)]
        public float DurationInSeconds { get; private set; } = 1f;
    }
}