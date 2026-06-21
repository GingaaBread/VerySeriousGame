using Main.Mono.Player;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace Main.Controller
{
    public class PlayerMovementController : Controller
    {
        [SerializeField] [Required] private PlayerMovement _playerMovement;

        private void OnDisable()
        {
            InputManager.Instance.InputMaster.Player.Move.performed -= TryMove;
            InputManager.Instance.InputMaster.Player.Move.canceled -= StopMove;
        }

        protected override void Subscribe()
        {
            InputManager.Instance.InputMaster.Player.Move.performed += TryMove;
            InputManager.Instance.InputMaster.Player.Move.canceled += StopMove;
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