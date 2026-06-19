using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Assertions;

namespace Utility.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Required]
        [SerializeField] private AudioInstance _managedPrefab;

        [SerializeField] private bool _dontDestroyOnLoad;


        private readonly Dictionary<AudioWrapper, AudioInstance> _createdInstances = new();
        public bool HasBeenRegistered { get; private set; }

        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "AudioManager singleton already exists");
            Instance = this;
            if (_dontDestroyOnLoad) DontDestroyOnLoad(gameObject);
        }

        private AudioInstance GetInstance([NotNull] AudioWrapper wrapper)
        {
            var exists = _createdInstances.TryGetValue(wrapper, out var instance);
            if (!exists)
                throw new InvalidOperationException($"Cannot receive {wrapper} because it has not been registered");

            return instance;
        }

        public void ReceiveEmittedEvent(AudioWrapper wrapper, AudioEventType eventType,
            NestedArray<AudioVariableReference> updatedVariables)
        {
            if (wrapper == null) throw new NullReferenceException("Audio wrapper has not been set in the inspector");

            var instance = GetInstance(wrapper);
            switch (eventType)
            {
                case AudioEventType.ACTIVATE:
                    instance.Activate();
                    break;
                case AudioEventType.DEACTIVATE:
                    instance.Stop();
                    break;
                case AudioEventType.VARIABLE_UPDATE:
                    instance.UpdateVariables(updatedVariables.Content);
                    break;
                case AudioEventType.ADVANCE:
                    instance.Advance();
                    break;
                case AudioEventType.SHEET_RESET:
                    instance.ResetSheet();
                    break;
                default:
                    throw new InvalidOperationException($"Event type {eventType} has not been implemented yet");
            }
        }

        public void Register(AudioWrapper[] audioWrappers)
        {
            HasBeenRegistered = true;
            TearDown();

            foreach (var audioWrapper in audioWrappers)
            {
                if (audioWrapper == null)
                    throw new NullReferenceException("Audio wrapper has not been set in the inspector");

#if UNITY_EDITOR
                Debug.Log($"Registering audio wrapper '{audioWrapper}'");
#endif
                if (_createdInstances.TryGetValue(audioWrapper, out var instance))
                    throw new InvalidOperationException($"Should not register an existing instance: {instance}");

                var createdInstance = CreateInstanceFor(audioWrapper);
                createdInstance.Register(audioWrapper);
            }

            foreach (var wrapper in audioWrappers.Where(wrapper => wrapper.AutoActivate))
            {
                var instance = GetInstance(wrapper);
                instance.Activate();
            }
        }

        private void TearDown()
        {
            foreach (var (_, value) in _createdInstances)
            {
                Destroy(value.gameObject);
            }

            _createdInstances.Clear();
        }

        private AudioInstance CreateInstanceFor(AudioWrapper wrapper)
        {
            var spawnedInstance = Instantiate(_managedPrefab, transform);
            _createdInstances.Add(wrapper, spawnedInstance);

            return spawnedInstance;
        }
    }
}