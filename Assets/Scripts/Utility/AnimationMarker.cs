using UnityEngine;
using UnityEngine.Events;

namespace Utility
{
    public class AnimationMarker : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onMark;

        public void Mark()
        {
            _onMark?.Invoke();
        }
    }
}