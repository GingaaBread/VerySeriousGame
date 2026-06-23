using System;
using UnityEngine;
using UnityEngine.UI;

namespace Utility.Transitions
{
    public class TransitionHandler : MonoBehaviour
    {
        [SerializeField] private Image _fadeImage;
        [SerializeField] private Animator _horizontalCurtainAnimator;
        [SerializeField] private Image[] _horizontalCurtainImages;

        private Action _notifier;

        public void Handle(Transition transition, TransitionEmitter.EmitterEvents events, Action notifier)
        {
            _notifier = notifier;
            ClearAll();

            events.OnCoverStart?.Invoke();
            PlayCover(transition, events);
        }

        private void PlayReveal(Transition transition, TransitionEmitter.EmitterEvents events)
        {
            var revealStyleTimeline = transition.Reveal;

            if (revealStyleTimeline == null || revealStyleTimeline.Style == null)
                throw new NullReferenceException("Reveal style is null");

            events.OnHoldStart?.Invoke();

            var revealStyle = revealStyleTimeline.Style;
            switch (revealStyle.StyleType)
            {
                case TransitionStyleType.FADE:
                    TweenFade(revealStyleTimeline, revealStyle.FadeColour, 1f, 0f)
                        .setDelay(transition.HoldDurationInSeconds)
                        .setIgnoreTimeScale(true)
                        .setOnStart(() => events.OnRevealStart?.Invoke())
                        .setOnComplete(() =>
                        {
                            _fadeImage.gameObject.SetActive(false);
                            events.OnComplete?.Invoke();
                            _notifier?.Invoke();
                        });
                    break;
                case TransitionStyleType.CURTAIN:
                    _horizontalCurtainAnimator.gameObject.SetActive(true);
                    _horizontalCurtainAnimator.Play("Reveal");
                    break;
                default:
                    throw new InvalidOperationException($"Style type '{revealStyle.StyleType}' is not yet implemented");
            }
        }

        private void PlayCover(Transition transition, TransitionEmitter.EmitterEvents events)
        {
            var coverStyleTimeline = transition.Cover;

            if (coverStyleTimeline == null || coverStyleTimeline.Style == null)
                throw new NullReferenceException("Cover style is null");

            var coverStyle = coverStyleTimeline.Style;
            switch (coverStyle.StyleType)
            {
                case TransitionStyleType.FADE:
                    _fadeImage.gameObject.SetActive(true);
                    TweenFade(coverStyleTimeline, coverStyle.FadeColour, 0f, 1f)
                        .setOnComplete(() => PlayReveal(transition, events));
                    break;
                case TransitionStyleType.CURTAIN:
                    // This should be a tween instead, and the reveal should be queued
                    _horizontalCurtainAnimator.gameObject.SetActive(true);
                    _horizontalCurtainAnimator.Play("Cover");
                    break;
                default:
                    throw new InvalidOperationException($"Style type '{coverStyle.StyleType}' is not yet implemented");
            }
        }

        private LTDescr TweenFade(StyleTimeline styleTimeline, Color colour, float from, float to)
        {
            colour.a = from;
            _fadeImage.color = colour;
            return LeanTween
                .value(from, to, styleTimeline.DurationInSeconds)
                .setOnUpdate(val =>
                {
                    colour.a = val;
                    _fadeImage.color = colour;
                })
                .setEase(styleTimeline.Ease);
        }

        private void ClearAll()
        {
            LeanTween.cancel(_fadeImage.gameObject);

            _fadeImage.gameObject.SetActive(false);
            _horizontalCurtainAnimator.gameObject.SetActive(false);
        }
    }
}