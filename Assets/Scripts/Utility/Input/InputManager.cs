using UnityEngine;

namespace Utility.Input
{
    /// <summary>
    ///     Manages the input master.
    /// </summary>
    public class InputManager : MonoBehaviour
    {
        /// <summary>
        ///     Provides access to the input master, allowing scripts to subscribe to input events
        /// </summary>
        public InputMaster InputMaster { private set; get; }

        /// <summary>
        ///     The singleton instance
        /// </summary>
        public static InputManager Instance { get; private set; }

        /// <summary>
        ///     Creates the singleton and input master
        /// </summary>
        private void Awake()
        {
            SetUpSingleton();
            CreateAndEnableInputMaster();
        }


        /// <summary>
        ///     Enables the input master
        /// </summary>
        private void OnEnable()
        {
            InputMaster.Enable();
        }

        /// <summary>
        ///     Disables the input master
        /// </summary>
        private void OnDisable()
        {
            InputMaster.Disable();
        }

        /// <summary>
        ///     Creates the input master used for subscriptions
        /// </summary>
        private void CreateAndEnableInputMaster()
        {
            InputMaster = new();
            InputMaster.Enable();
        }

        /// <summary>
        ///     Creates the singleton instance
        /// </summary>
        private void SetUpSingleton()
        {
            if (Instance != null) return;
            Instance = this;
        }
    }
}