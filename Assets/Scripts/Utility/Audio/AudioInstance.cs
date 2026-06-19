using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioInstance : MonoBehaviour
    {
        [Required]
        [SerializeField] private AudioSource _reference;

        private readonly Dictionary<AudioVariable, object> _variableStates = new();


        private Queue<AudioClip> _currentClipSequence;
        private AudioSheet _currentSheet;
        private bool _hasBeenRegistered;

        private AudioWrapper _representedWrapper;

        public void Register(AudioWrapper wrapper)
        {
            if (_hasBeenRegistered)
                throw new InvalidOperationException("An audio instance can only be registered once");

            _hasBeenRegistered = true;

            SetSheet(wrapper.MainSheet);
            RegisterVariables(wrapper);
        }

        private void SetSheet(AudioSheet sheet)
        {
            _currentSheet = sheet;

            if (sheet.Clips.Length <= 1) return;

            switch (sheet.ClipUsage)
            {
                case MultipleClipUsage.SEQUENCE:
                    AssignSequence();
                    break;
                case MultipleClipUsage.RANDOMIZATION:
                    AssignBaggedSequence();
                    break;
                case MultipleClipUsage.NONE:
                    break;
                case MultipleClipUsage.LAYERING:
                    break;
                default:
                    throw new InvalidOperationException($"Clip usage {sheet.ClipUsage} is not yet supported");
            }
        }

        private void AssignSequence()
        {
            if (_currentSheet.ClipUsage is MultipleClipUsage.SEQUENCE && _currentSheet.Clips.Length > 1)
                _currentClipSequence = new(_currentSheet.Clips);
            else throw new InvalidOperationException("Should not assign sequence");
        }

        private void AssignBaggedSequence()
        {
            if (_currentSheet.ClipUsage is MultipleClipUsage.RANDOMIZATION && _currentSheet.GuaranteeAll &&
                _currentSheet.Clips.Length > 1)
            {
                /*
                var last = _currentClipSequence?[^1];
                _currentClipSequence = _currentSheet.Clips;
                ShuffleService.Shuffle(_currentClipSequence);

                if (_currentSheet == null || last != _currentClipSequence[0]) return;

                var randomIndex = Random.Range(1, _currentClipSequence.Length);
                (_currentClipSequence[0], _currentClipSequence[randomIndex]) =
                    (_currentClipSequence[randomIndex], _currentClipSequence[0]);*/
            }
            else
            {
                _currentClipSequence = null;
            }
        }

        private void RegisterVariables(AudioWrapper wrapper)
        {
            _representedWrapper = wrapper;
            var variables = wrapper.Variables;
            foreach (var audioVariable in variables)
            {
                _variableStates.Add(audioVariable, audioVariable.InitialValue());
            }
        }

        public void Activate()
        {
            if (!PlaysOnActivation()) return;
            PlayInCurrentState();
        }

        private bool PlaysOnActivation() =>
            !(_currentSheet.ClipUsage is MultipleClipUsage.SEQUENCE && _currentSheet.Clips.Length > 1);

        private void PlayInCurrentState()
        {
            SetClip(_currentSheet);
            SetLoop(_currentSheet);
            _reference.Play();
        }

        public void Stop()
        {
            _reference.Stop();
        }

        private void SetClip(AudioSheet currentSheet)
        {
            _reference.clip = currentSheet.ClipUsage switch
            {
                MultipleClipUsage.NONE => currentSheet.Clips[0],
                MultipleClipUsage.RANDOMIZATION when currentSheet.WeightedRandomization =>
                    WeightService.Draw(currentSheet.Clips, currentSheet.RandomizationWeights.Content),
                MultipleClipUsage.RANDOMIZATION when !currentSheet.WeightedRandomization =>
                    currentSheet.Clips[Random.Range(0, currentSheet.Clips.Length)],
                MultipleClipUsage.LAYERING => throw new InvalidOperationException("Layering is not yet supported"),
                MultipleClipUsage.SEQUENCE => Peek(),
                _ => throw new InvalidOperationException(
                    $"The selected multiple clip usage '{currentSheet.ClipUsage}'" +
                    " cannot be played because it has not yet been implemented.")
            };
        }

        private AudioClip Peek()
        {
            if (_currentClipSequence?.Count is not 0) return _currentClipSequence?.Peek();

            Debug.LogWarning("Cannot advance the sequence because there are no elements left. " +
                             $"Wrapper: {_representedWrapper}");
            return null;
        }

        private void SetLoop(AudioSheet currentSheet)
        {
            _reference.loop = currentSheet.LoopBehaviour switch
            {
                LoopingBehaviour.NO_LOOP => false,
                LoopingBehaviour.SIMPLE_LOOP => true,
                _ => throw new InvalidOperationException(
                    $"The selected looping behaviour '{currentSheet.LoopBehaviour}" +
                    "' cannot be played because it has not yet been implemented.")
            };
        }

        public void UpdateVariables(IEnumerable<AudioVariableReference> variables)
        {
            foreach (var reference in variables)
            {
                var res = _variableStates.Keys.FirstOrDefault(key => key.Id == reference.VariableName);
                if (res == null)
                    throw new KeyNotFoundException($"Cannot update variable name {reference.VariableName} " +
                                                   $"of wrapper {_representedWrapper} because the variable does not exist");

                _variableStates[res] = reference.Value();
            }

            ProcessState();
        }

        private void ProcessState()
        {
            var found = false;
            foreach (var transition in _currentSheet.Transitions)
            {
                var variable = _variableStates
                    .FirstOrDefault(v =>
                    {
                        if (v.Key.Id != transition.If.VariableId || v.Key.VariableType is not AudioVariableType.BOOL)
                            return false;

                        if (transition.If.Not) return v.Value is false;
                        return v.Value is true;
                    });

                if (variable.Key == null) continue;

                if (found)
                {
                    Debug.LogWarning(
                        $"Found multiple eligible transitions for {_representedWrapper}. Choosing the first.");
                    return;
                }

                found = true;
                var sheetName = transition.To;
                var sheetExists = _representedWrapper.TryGetSheet(sheetName, out var sheet);

                if (!sheetExists)
                    throw new KeyNotFoundException($"Cannot transition from sheet '{_currentSheet.Id}' " +
                                                   $"into '{sheetName}' because it does not exist in the wrapper " +
                                                   $"(Transition: '{transition.Id}' of Wrapper '{_representedWrapper}')");

                TransitionInto(sheet, transition.TransitionType);
            }
        }

        private void TransitionInto(AudioSheet sheet, AudioTransitionType transitionType)
        {
            if (transitionType is not AudioTransitionType.IMMEDIATE)
                throw new InvalidOperationException("Transition type is not yet supported");

            SetSheet(sheet);
            PlayInCurrentState();
        }

        public void Advance()
        {
            if (_currentSheet.Clips.Length <= 1 || _currentSheet.ClipUsage is not MultipleClipUsage.SEQUENCE)
            {
                PlayInCurrentState();
                return;
            }

            if (_currentClipSequence == null) AssignSequence();

            if (_currentClipSequence?.Count is 0)
                if (_currentSheet.LoopBehaviour is LoopingBehaviour.SIMPLE_LOOP)
                {
                    AssignSequence();
                }
                else
                {
                    Debug.LogWarning("Cannot advance the sequence because there are no elements left. " +
                                     $"(Sheet {_currentSheet} of wrapper: {_representedWrapper})");
                    return;
                }


            PlayInCurrentState();
            _currentClipSequence?.Dequeue();
        }

        public void ResetSheet()
        {
            SetSheet(_currentSheet);
        }
    }
}