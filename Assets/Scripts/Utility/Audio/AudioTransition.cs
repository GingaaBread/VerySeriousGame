using System;
using NaughtyAttributes;
using UnityEngine;

namespace Utility.Audio
{
    [Serializable]
    public class AudioTransition
    {
        [field: BoxGroup("Transitions")]
        [field: AllowNesting]
        [field: SerializeField]
        public string Id { get; private set; }

        [field: BoxGroup("Transitions")]
        [field: AllowNesting]
        [field: SerializeField]
        public AudioTransitionType TransitionType { get; private set; }

        [field: BoxGroup("Transitions")]
        [field: AllowNesting]
        [field: InfoBox("The name of the sheet to transition into")]
        [field: SerializeField]
        public string To { get; private set; } = "Main Sheet";

        [field: BoxGroup("Transitions")]
        [field: AllowNesting]
        [field: SerializeField]
        public Condition If { get; private set; }

        [Serializable]
        public class Condition
        {
            [field: AllowNesting]
            [field: InfoBox("Invert the variable condition")]
            [field: SerializeField]
            public bool Not { get; private set; }

            [field: AllowNesting]
            [field: InfoBox("The name of the variable")]
            [field: SerializeField]
            public string VariableId { get; private set; } = "Variable";

            public override string ToString() => VariableId;
        }
    }
}