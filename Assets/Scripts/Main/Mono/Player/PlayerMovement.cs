using Main.Service;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;

namespace Main.Mono.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMovement : MonoBehaviour
    {
        private static readonly int IS_MOVING_ID = Animator.StringToHash("Is Walking");
        private static readonly int IS_DRILLING_ID = Animator.StringToHash("Is Drilling");
        [SerializeField] private float _defaultMovementSpeed = 5f;
        [SerializeField] private bool _isAllowedToMoveUp;
        [SerializeField] private bool _isAllowedToMoveDown = true;
        [SerializeField] private Animator _animator;

        [Inject] private readonly DrillActivationMediator _drillActivationMediator;
        private Vector2 _currentMovementData;
        private Rigidbody2D _rigidbody2D;

        private bool _wasMoving;

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            var isIdle = Mathf.Approximately(0f, _rigidbody2D.linearVelocity.x) &&
                         Mathf.Approximately(0f, _rigidbody2D.linearVelocity.y);
            var isMoving = !isIdle;

            if (isMoving == _wasMoving) return;

            _animator.SetBool(IS_MOVING_ID, isMoving);
            _wasMoving = isMoving;
        }

        private void FixedUpdate()
        {
            _rigidbody2D.linearVelocity = _currentMovementData * _defaultMovementSpeed;
        }

        private void OnEnable()
        {
            if (_drillActivationMediator == null) return;
            _drillActivationMediator.OnActivationChange += UpdateDrillAnimator;
        }

        private void OnDisable()
        {
            if (_drillActivationMediator == null) return;
            _drillActivationMediator.OnActivationChange -= UpdateDrillAnimator;
        }

        private void UpdateDrillAnimator(bool isDrilling)
        {
            _animator.SetBool(IS_DRILLING_ID, isDrilling);
        }

        public void TryMove(InputAction.CallbackContext ctx)
        {
            var direction = ctx.ReadValue<Vector2>().normalized;

            if (!_isAllowedToMoveUp) direction.y = Mathf.Min(0f, direction.y);
            if (!_isAllowedToMoveDown) direction.y = Mathf.Max(0f, direction.y);

            _currentMovementData = direction;
        }

        public void StopMoving()
        {
            _currentMovementData = Vector2.zero;
            _rigidbody2D.linearVelocity = Vector2.zero;
        }
    }
}