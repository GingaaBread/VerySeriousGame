using UnityEngine;
using UnityEngine.InputSystem;

namespace Main.Mono.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private float _defaultMovementSpeed = 5f;
        [SerializeField] private bool _isAllowedToMoveUp;
        private Vector2 _currentMovementData;

        private Rigidbody2D _rigidbody2D;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            _rigidbody2D.linearVelocity = _currentMovementData * _defaultMovementSpeed;
        }

        public void TryMove(InputAction.CallbackContext ctx)
        {
            var direction = ctx.ReadValue<Vector2>().normalized;

            if (!_isAllowedToMoveUp) direction.y = Mathf.Min(0f, direction.y);

            _currentMovementData = direction;
        }

        public void StopMoving()
        {
            _currentMovementData = Vector2.zero;
            _rigidbody2D.linearVelocity = Vector2.zero;
        }
    }
}