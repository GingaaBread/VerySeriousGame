using Main.Mono.Player;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace Main.Controller
{
    public class PlayerMovementController : MonoBehaviour
    {
        [SerializeField] [Required] private PlayerMovement _playerMovement;

        private void OnEnable()
        {
            InputManager.Instance.InputMaster.Player.Move.performed += TryMove;
            InputManager.Instance.InputMaster.Player.Move.canceled += StopMove;
        }

        private void OnDestroy()
        {
            InputManager.Instance.InputMaster.Player.Move.performed -= TryMove;
            InputManager.Instance.InputMaster.Player.Move.canceled -= StopMove;
        }

        private void StopMove(InputAction.CallbackContext _)
        {
            _playerMovement.StopMoving();
        }

        private void TryMove(InputAction.CallbackContext ctx)
        {
            _playerMovement.TryMove(ctx);
        }
    }
}