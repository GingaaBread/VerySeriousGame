using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Transitions;

namespace Main.Mono.Player
{
    public class SurfaceReachTracker : MonoBehaviour
    {
        [SerializeField] private Transform _surfaceTransform;
        [SerializeField] private TransitionEmitter _transitionEmitter;

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
            Debug.Log("Loading surface");
            _isLoading = true;
            _transitionEmitter.Emit();
        }

        public void LoadScene()
        {
            SceneManager.LoadScene("Surface");
        }
    }
}