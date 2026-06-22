using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using Utility;

namespace Main.Controller
{
    public class ExitController : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onExit;
        private Coroutine _coroutine;

        private float _lastExitTime;

        private void OnEnable()
        {
            if (_coroutine != null) return;
            _coroutine = StartCoroutine(Subscribe());
        }

        private void OnDisable()
        {
            InputManager.Instance.InputMaster.UI.Cancel.performed -= Exit;
        }

        private IEnumerator Subscribe()
        {
            yield return new WaitForSeconds(0.1f);
            InputManager.Instance.InputMaster.UI.Cancel.performed += Exit;
            _coroutine = null;
        }

        private void Exit(InputAction.CallbackContext _)
        {
            if (Time.unscaledTime - _lastExitTime < 0.1f) return;
            _lastExitTime = Time.unscaledTime;
            _onExit?.Invoke();
        }
    }
}