using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

namespace Utility.Transitions
{
    public class TransitionManager : MonoBehaviour
    {
        [SerializeField] private TransitionHandler _handler;

        private (Transition, TransitionEmitter.EmitterEvents) _current;

        private bool _isPlaying;
        private List<(Transition, TransitionEmitter.EmitterEvents)> _priorityQueue = new();
        private List<Transition> _registeredTransitions;

        public static TransitionManager Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "TransitionManager singleton already exists");
            Instance = this;
        }

        public void Register(IEnumerable<Transition> transitions)
        {
            if (_registeredTransitions != null)
                throw new InvalidOperationException("Cannot register transitions multiple times");

            _registeredTransitions = transitions.ToList();
        }

        public void Receive(Transition transition, TransitionEmitter.EmitterEvents events)
        {
            if (transition == null)
                throw new NullReferenceException("Transition cannot be null");

            if (!_registeredTransitions.Contains(transition))
                Debug.LogWarning($"Should not use the unregistered transition '{transition}'");

            Add(transition, events);
        }

        private void Add(Transition transition, TransitionEmitter.EmitterEvents events)
        {
            switch (transition.Policy)
            {
                case QueuePolicy.ENQUEUE:
                    Enqueue(transition, events);
                    break;
                case QueuePolicy.REJECT:
                    if (_priorityQueue.Count is 0) Enqueue(transition, events);
                    break;
                case QueuePolicy.INTERRUPT:
                    if (_current.Item1 != null && _current.Item1.DoNotInterrupt)
                    {
                        Enqueue(transition, events);
                    }
                    else
                    {
                        _priorityQueue.Insert(0, (transition, events));
                        PlayNext();
                    }

                    break;
                default:
                    throw new InvalidOperationException($"Transition policy '{transition.Policy}' has " +
                                                        "not been implemented yet");
            }
        }

        private void Enqueue(Transition transition, TransitionEmitter.EmitterEvents emitterEvents)
        {
            _priorityQueue.Add((transition, emitterEvents));

            _priorityQueue = _priorityQueue.OrderBy(t => t.Item1.Priority).ToList();

            if (!_isPlaying) PlayNext();
        }

        private void PlayNext()
        {
            _isPlaying = true;
            _current = _priorityQueue[0];
            _priorityQueue.RemoveAt(0);
            _handler.Handle(_current.Item1, _current.Item2, MarkCompletion);
        }

        private void MarkCompletion()
        {
            if (_priorityQueue.Count is 0)
            {
                _isPlaying = false;
                return;
            }

            PlayNext();
        }
    }
}