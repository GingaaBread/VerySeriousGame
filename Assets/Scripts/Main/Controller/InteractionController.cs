using Main.Service;
using UnityEngine.InputSystem;
using Utility;
using VContainer;

namespace Main.Controller
{
    public class InteractionController : Controller
    {
        [Inject] private readonly InteractionService _interactionService;

        private void OnDisable()
        {
            InputManager.Instance.InputMaster.Player.Interact.performed -= TryInteract;
        }

        protected override void Subscribe()
        {
            InputManager.Instance.InputMaster.Player.Interact.performed += TryInteract;
        }

        private void TryInteract(InputAction.CallbackContext _)
        {
            // Only the triggers the first now
            _interactionService.Trigger(0);
        }
    }
}