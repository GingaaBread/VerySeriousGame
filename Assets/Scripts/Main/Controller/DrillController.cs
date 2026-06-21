using Main.Service;
using UnityEngine.InputSystem;
using Utility;
using VContainer;

namespace Main.Controller
{
    public class DrillController : Controller
    {
        [Inject] private readonly DrillService _drillService;

        private void OnDisable()
        {
            InputManager.Instance.InputMaster.Player.ToggleDrill.performed -= TryToggleDrill;
        }

        private void TryToggleDrill(InputAction.CallbackContext _)
        {
            _drillService.ToggleActivity();
        }

        protected override void Subscribe()
        {
            InputManager.Instance.InputMaster.Player.ToggleDrill.performed += TryToggleDrill;
        }
    }
}