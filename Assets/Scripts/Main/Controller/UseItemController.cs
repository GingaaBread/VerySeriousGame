using Main.Entity;
using Main.Mono.Player;
using Main.Service;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using VContainer;

namespace Main.Controller
{
    public class UseItemController : Controller
    {
        [SerializeField] private TeleportationManager _teleportationManager;

        [SerializeField] private ItemSo _usableItem;

        private bool _forbidTeleport;
        [Inject] private PlayerInventoryService _playerInventoryService;

        private void OnDisable()
        {
            InputManager.Instance.InputMaster.Player.UseItem.performed -= TryUseItem;
        }

        protected override void Subscribe()
        {
            InputManager.Instance.InputMaster.Player.UseItem.performed += TryUseItem;
        }

        private void TryUseItem(InputAction.CallbackContext _)
        {
            if (!_playerInventoryService.CanRemove(_usableItem, 1) || _forbidTeleport) return;
            _forbidTeleport = true;
            _playerInventoryService.Remove(_usableItem, 1);
            _teleportationManager.Teleport();
        }
    }
}