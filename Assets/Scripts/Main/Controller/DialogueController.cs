using Main.View;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;

namespace Main.Controller
{
    public class DialogueController : Controller
    {
        [SerializeField] private DialogueCanvas _dialogueCanvas;

        private void OnDisable()
        {
            InputManager.Instance.InputMaster.Dialogue.Continue.performed -= Proceed;
            InputManager.Instance.InputMaster.Dialogue.Skip.performed -= Skip;
        }

        protected override void Subscribe()
        {
            InputManager.Instance.InputMaster.Dialogue.Continue.performed += Proceed;
            InputManager.Instance.InputMaster.Dialogue.Skip.performed += Skip;
        }


        private void Skip(InputAction.CallbackContext _)
        {
            _dialogueCanvas.HandleCompletedDialogue();
        }

        private void Proceed(InputAction.CallbackContext _)
        {
            _dialogueCanvas.TryProceed();
        }
    }
}