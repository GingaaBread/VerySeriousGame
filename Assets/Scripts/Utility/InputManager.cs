using UnityEngine;
using UnityEngine.Assertions;

namespace Utility
{
    public class InputManager : MonoBehaviour
    {
        public InputMaster InputMaster { get; private set; }

        public static InputManager Instance { get; private set; }

        private void Awake()
        {
            Assert.IsNull(Instance, "InputManager singleton already exists");
            Instance = this;
            InputMaster = new();
        }

        private void OnEnable()
        {
            InputMaster.Enable();
        }

        private void OnDisable()
        {
            InputMaster.Disable();
        }
    }
}