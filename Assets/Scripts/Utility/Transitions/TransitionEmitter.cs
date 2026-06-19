using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utility.Transitions
{
    public class TransitionEmitter : MonoBehaviour
    {
        [SerializeField] private Transition _transitionReference;

        [field: SerializeField] public EmitterEvents Events { get; private set; }

        public void Emit()
        {
            TransitionManager.Instance.Receive(_transitionReference, Events);
        }

        [Serializable]
        public class EmitterEvents
        {
            [field: SerializeField] public UnityEvent OnCoverStart { get; private set; }
            [field: SerializeField] public UnityEvent OnHoldStart { get; private set; }
            [field: SerializeField] public UnityEvent OnRevealStart { get; private set; }
            [field: SerializeField] public UnityEvent OnComplete { get; private set; }
        }
    }
}