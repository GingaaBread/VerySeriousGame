using System;
using NaughtyAttributes;
using UnityEngine;

namespace Utility.Audio
{
    [Serializable]
    public class AudioVariable : IValidated
    {
        [field: SerializeField]
        public string Id { get; private set; } = "New Variable";

        [field: SerializeField]
        public AudioVariableType VariableType { get; private set; }

        [field: AllowNesting]
        [field: ShowIf(nameof(ShowBoolProperties))]
        [field: SerializeField]
        public bool InitialBoolValue { get; private set; }

        [field: AllowNesting]
        [field: SerializeField]
        [field: ShowIf(nameof(ShowEnumerationProperties))]
        public NestedArray<string> EnumerationConstants { get; private set; }

        [field: AllowNesting]
        [field: SerializeField]
        [field: ShowIf(nameof(ShowEnumerationProperties))]
        public int InitialEnumerationValue { get; private set; }

        [field: AllowNesting]
        [field: SerializeField]
        [field: ShowIf(nameof(ShowIntegerProperties))]
        public int InitialIntegerValue { get; private set; }

        [field: AllowNesting]
        [field: SerializeField]
        [field: ShowIf(nameof(ShowFloatingProperties))]
        public float InitialFloatValue { get; private set; }

        private bool ShowBoolProperties => VariableType is AudioVariableType.BOOL;
        private bool ShowEnumerationProperties => VariableType is AudioVariableType.ENUMERATION;
        private bool ShowIntegerProperties => VariableType is AudioVariableType.INTEGER;
        private bool ShowFloatingProperties => VariableType is AudioVariableType.FLOAT;

        public bool Validate()
        {
            if (!ShowBoolProperties) InitialBoolValue = false;

            if (!ShowEnumerationProperties)
            {
                EnumerationConstants = new(Array.Empty<string>());
                InitialEnumerationValue = 0;
            }

            if (!ShowIntegerProperties) InitialIntegerValue = 0;

            if (!ShowFloatingProperties) InitialFloatValue = 0f;

            return true;
        }

        public object InitialValue() => VariableType switch
        {
            AudioVariableType.INTEGER => InitialIntegerValue,
            AudioVariableType.FLOAT => InitialFloatValue,
            AudioVariableType.BOOL => InitialBoolValue,
            AudioVariableType.ENUMERATION => InitialEnumerationValue,
            _ => throw new InvalidOperationException($"Variable type {VariableType} has not been implemented")
        };
    }
}