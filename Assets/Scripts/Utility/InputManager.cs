using UnityEngine;
using UnityEngine.Assertions;

namespace Utility
{
    public class InputManager : MonoBehaviour
    {
        private int _uiCount;
        public InputMaster InputMaster { get; private set; }

        public static InputManager Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "InputManager singleton already exists");
            Instance = this;
            InputMaster = new();
            InputMaster.UI.Disable();
        }

        private void OnEnable()
        {
            InputMaster.Enable();
        }

        private void OnDisable()
        {
            InputMaster.Disable();
        }

        public void PushUI()
        {
            _uiCount++;
            if (_uiCount == 1) InputMaster.UI.Enable();
        }

        public void PopUI()
        {
            _uiCount--;
            if (_uiCount == 0) InputMaster.UI.Disable();
        }
    }
}