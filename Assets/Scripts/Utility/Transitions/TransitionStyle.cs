using NaughtyAttributes;
using UnityEngine;

namespace Utility.Transitions
{
    [CreateAssetMenu(fileName = "Transition Style", menuName = "Transition/Style", order = 0)]
    public class TransitionStyle : ScriptableObject
    {
        [field: SerializeField] public TransitionStyleType StyleType { get; private set; }

        [field: BoxGroup("Fade Properties")]
        [field: ShowIf(nameof(ShouldShowFadeProperties))]
        [field: SerializeField]
        public Color FadeColour { get; private set; } = Color.black;

        private bool ShouldShowFadeProperties => StyleType is TransitionStyleType.FADE;
    }
}