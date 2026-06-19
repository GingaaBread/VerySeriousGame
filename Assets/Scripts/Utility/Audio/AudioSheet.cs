using System;
using NaughtyAttributes;
using UnityEngine;

namespace Utility.Audio
{
    [Serializable]
    public class AudioSheet : IValidated
    {
        [field: InfoBox("The id of the sheet must be unique!")]
        [field: SerializeField] public string Id { get; private set; } = "Main Sheet";

        [field: AllowNesting]
        [field: InfoBox("The audio clips this wrapper processes. Cannot be empty.")]
        [field: ValidateInput(nameof(IsValidClipsProperty), "Must have at least one clip. " +
                                                            "Otherwise, you can remove the wrapper.")]
        [field: SerializeField] public AudioClip[] Clips { get; private set; }

        [field: AllowNesting]
        [field: ShowIf(nameof(ShouldShowClipUsage))]
        [field: SerializeField] public MultipleClipUsage ClipUsage { get; private set; } = MultipleClipUsage.NONE;

        [field: BoxGroup("Randomization")]
        [field: AllowNesting]
        [field: ShowIf(nameof(ShouldShowRandomizationProperties))]
        [field: SerializeField] public bool GuaranteeAll { get; private set; }

        [field: BoxGroup("Randomization")]
        [field: AllowNesting]
        [field: ShowIf(nameof(ShouldShowRandomizationWeights))]
        [field: SerializeField]
        public NestedArray<int> RandomizationWeights { get; private set; }

        [field: BoxGroup("Randomization")]
        [field: AllowNesting]
        [field: ShowIf(nameof(ShouldShowRandomizationProperties))]
        [field: SerializeField] public bool WeightedRandomization { get; private set; }

        [field: HorizontalLine]
        [field: InfoBox("Determines if and how the instance loops (repeats after reaching a point)")]
        [field: SerializeField] public LoopingBehaviour LoopBehaviour { get; private set; }

        [field: BoxGroup("Transitions")]
        [field: AllowNesting]
        [field: SerializeField] public AudioTransition[] Transitions { get; private set; }

        private bool ShouldShowRandomizationProperties => ClipUsage is MultipleClipUsage.RANDOMIZATION;

        private bool ShouldShowClipUsage => Clips?.Length > 1;

        private bool ShouldShowRandomizationWeights =>
            ClipUsage is MultipleClipUsage.RANDOMIZATION && WeightedRandomization;

        public bool Validate()
        {
            EnsureWeightSize();
            if (!ShouldShowClipUsage) ClipUsage = MultipleClipUsage.NONE;

            return true;
        }

        // ReSharper disable once SuggestBaseTypeForParameter
        private bool IsValidClipsProperty(AudioClip[] clips) => clips.Length > 0;

        private void EnsureWeightSize()
        {
            var weights = RandomizationWeights.Content;

            if (!ShouldShowRandomizationWeights || weights.Length == Clips.Length) return;

            var resized = new int[Clips.Length];
            var copyLength = Mathf.Min(weights.Length, Clips.Length);

            Array.Copy(weights, resized, copyLength);

            for (var i = copyLength; i < Clips.Length; i++)
                resized[i] = 1;

            RandomizationWeights = new(resized);
        }
    }
}