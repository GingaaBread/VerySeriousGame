using Main.Service;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using VContainer;

namespace Main.Controller
{
    public class DrillController : MonoBehaviour
    {
        [Inject] private readonly DrillService _drillService;

        private void OnEnable()
        {
            InputManager.Instance.InputMaster.Player.ToggleDrill.performed += TryToggleDrill;
        }

        private void OnDisable()
        {
            InputManager.Instance.InputMaster.Player.ToggleDrill.performed -= TryToggleDrill;
        }

        private void TryToggleDrill(InputAction.CallbackContext _)
        {
            _drillService.ToggleActivity();
        }
    }
}