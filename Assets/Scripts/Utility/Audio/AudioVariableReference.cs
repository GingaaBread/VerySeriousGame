using System;
using NaughtyAttributes;
using UnityEngine;

namespace Utility.Audio
{
    [Serializable]
    public class AudioVariableReference
    {
        [field: SerializeField] public string VariableName { get; private set; }
        [field: SerializeField] public AudioVariableType VariableType { get; private set; }

        [field: AllowNesting]
        [field: SerializeField]
        [field: ShowIf(nameof(ShouldShowIntegerProperties))]
        public int IntegerValue { get; private set; }

        [field: AllowNesting]
        [field: SerializeField]
        [field: ShowIf(nameof(ShouldShowFloatProperties))]
        public float FloatValue { get; private set; }

        [field: AllowNesting]
        [field: SerializeField]
        [field: ShowIf(nameof(ShouldShowBoolProperties))]
        public bool BoolValue { get; private set; }

        [field: AllowNesting]
        [field: SerializeField]
        [field: ShowIf(nameof(ShouldShowEnumerationProperties))]
        public string EnumerationConstantValue { get; private set; }

        private bool ShouldShowIntegerProperties => VariableType is AudioVariableType.INTEGER;
        private bool ShouldShowFloatProperties => VariableType is AudioVariableType.FLOAT;
        private bool ShouldShowBoolProperties => VariableType is AudioVariableType.BOOL;
        private bool ShouldShowEnumerationProperties => VariableType is AudioVariableType.ENUMERATION;

        public object Value()
        {
            return VariableType switch
            {
                AudioVariableType.INTEGER => IntegerValue,
                AudioVariableType.ENUMERATION => EnumerationConstantValue,
                AudioVariableType.BOOL => BoolValue,
                AudioVariableType.FLOAT => FloatValue,
                _ => throw new InvalidOperationException($"Value {VariableType} not yet implemented")
            };
        }
    }
}