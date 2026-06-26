using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utility
{
    [RequireComponent(typeof(Animator))]
    public class RandomisedAnimation : MonoBehaviour
    {
        [Required] [SerializeField] private Animator _animator;
        [SerializeField] private string _stateName;

        private void Awake()
        {
            PlayRandomised();
        }

        private void PlayRandomised()
        {
            var randomTime = Random.value; // 0.0 - 1.0
            _animator.Play(_stateName, 0, randomTime);
            _animator.Update(0f); // Immediately apply the new state
        }
    }
}