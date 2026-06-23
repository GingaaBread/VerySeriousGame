using UnityEngine;
using UnityEngine.Events;

namespace Main.Mono.Player
{
    public class SurfaceReachTracker : MonoBehaviour
    {
        [SerializeField] private Transform _surfaceTransform;
        [SerializeField] private UnityEvent _onSurfaceReached;

        private bool _isLoading;

        private void Start()
        {
            _isLoading = false;
        }

        private void Update()
        {
            if (transform.position.y <= _surfaceTransform.position.y || _isLoading) return;
            LoadSurface();
        }

        private void LoadSurface()
        {
            _isLoading = true;
            _onSurfaceReached?.Invoke();
        }
    }
}