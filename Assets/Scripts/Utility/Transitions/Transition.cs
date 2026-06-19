using NaughtyAttributes;
using UnityEngine;

namespace Utility.Transitions
{
    [CreateAssetMenu(fileName = "Transition", menuName = "Transition/Transition", order = 0)]
    public class Transition : ScriptableObject
    {
        [field: SerializeField]
        [field: ReadOnly]
        public string Id { get; private set; }

        [field: HorizontalLine]
        [field: SerializeField]
        [field: Min(1)]
        [field: Header("Settings")]
        public int Priority { get; private set; } = 1;

        [field: SerializeField]
        public QueuePolicy Policy { get; private set; }

        [field: SerializeField]
        public bool DoNotInterrupt { get; private set; }

        [field: SerializeField]
        public string[] Labels { get; private set; }

        [field: HorizontalLine]
        [field: SerializeField]
        [field: Header("Styles")]
        public StyleTimeline Cover { get; private set; }

        [field: SerializeField]
        [field: Min(0f)]
        public float HoldDurationInSeconds { get; private set; } = 1f;

        [field: SerializeField]
        public StyleTimeline Reveal { get; private set; }

        private void OnValidate()
        {
            Id = name;
        }

        public override string ToString() => Id;
    }
}