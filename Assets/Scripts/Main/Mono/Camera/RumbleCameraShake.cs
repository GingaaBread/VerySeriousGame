using Unity.Cinemachine;
using UnityEngine;

namespace Main.Mono.Camera
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class RumbleCameraShake : MonoBehaviour
    {
        [Min(0f)]
        [SerializeField] private float _strength = 0.1f;

        [Min(0f)]
        [SerializeField] private float _interval = 0.1f;

        private CinemachineImpulseSource _impulseSource;
        private bool _shouldApply;
        private float _timer;

        private void Awake()
        {
            _impulseSource = GetComponent<CinemachineImpulseSource>();
        }

        private void Update()
        {
            if (!_shouldApply) return;

            _timer += Time.deltaTime;

            if (_timer < _interval) return;

            _timer = 0f;
            _impulseSource.GenerateImpulse(_strength);
        }

        private void OnEnable() => _shouldApply = true;

        private void OnDisable() => _shouldApply = false;
    }
}