using System.Collections;
using UnityEngine;

namespace Utility
{
    [RequireComponent(typeof(Animator))]
    public class DelayedAnimation : MonoBehaviour
    {
        [Min(0f)]
        [SerializeField] private float _delayInSeconds;

        [SerializeField] private string _animationNameToPlay;
        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            Animate();
        }

        private void Animate()
        {
            StartCoroutine(WaitThenAnimate());
        }

        private IEnumerator WaitThenAnimate()
        {
            yield return new WaitForSeconds(_delayInSeconds);
            _animator.Play(_animationNameToPlay);
        }
    }
}