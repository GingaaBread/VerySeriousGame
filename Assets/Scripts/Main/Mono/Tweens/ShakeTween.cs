using UnityEngine;

namespace Main.Mono.Tweens
{
    public class ShakeTween : MonoBehaviour
    {
        private const int INFINITE_LOOP_KEY = -1;

        [Min(0f)]
        [SerializeField] private float _time = 0.1f;

        private Quaternion _initialRotation;

        private void OnEnable()
        {
            _initialRotation = transform.rotation;
            var originalPos = transform.localPosition;

            LeanTween.rotateAroundLocal(gameObject, Vector3.forward, 15f, _time)
                .setEase(LeanTweenType.easeShake)
                .setLoopCount(INFINITE_LOOP_KEY)
                .setLoopType(LeanTweenType.pingPong);

            LeanTween.moveLocalX(gameObject, originalPos.x + 0.1f, _time)
                .setEase(LeanTweenType.easeShake)
                .setLoopCount(INFINITE_LOOP_KEY)
                .setLoopType(LeanTweenType.pingPong);
        }

        private void OnDisable()
        {
            transform.rotation = _initialRotation;
            LeanTween.cancel(gameObject);
        }
    }
}