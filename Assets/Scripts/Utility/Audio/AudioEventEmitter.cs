using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Utility.Audio
{
    public class AudioEventEmitter : MonoBehaviour
    {
        [InfoBox("The wrapper that will have its state altered")]
        [SerializeField] private AudioWrapper _wrapperReference;

        [InfoBox("Determines what the event does")]
        [SerializeField] private AudioEventType _eventType;


        [SerializeField] private bool _emitDelayed;

        [Min(0)]
        [ShowIf(nameof(_emitDelayed))]
        [SerializeField] private float _delayInSeconds;

        [AllowNesting]
        [ShowIf(nameof(ShowVariableProperties))]
        [SerializeField] private NestedArray<AudioVariableReference> _updatedVariables;

        [Header("Unity Lifecycle")]
        [SerializeField] private bool _emitOnEnable;

        [SerializeField] private bool _emitOnStart;

        private bool ShowVariableProperties => _eventType is AudioEventType.VARIABLE_UPDATE;

        private IEnumerator Start()
        {
            yield return new WaitUntil(() => AudioManager.Instance.HasBeenRegistered);
            if (_emitOnStart) Emit();
        }

        private void OnEnable()
        {
            if (_emitOnEnable) Emit();
        }

        public void Emit()
        {
            if (_emitDelayed) StartCoroutine(DelayAndSend());
            else Send();
        }

        private IEnumerator DelayAndSend()
        {
            yield return new WaitForSecondsRealtime(_delayInSeconds);
            Send();
        }

        private void Send()
        {
            AudioManager.Instance.ReceiveEmittedEvent(_wrapperReference, _eventType, _updatedVariables);
        }
    }
}