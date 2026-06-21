using System.Collections;
using UnityEngine;
using Utility;

namespace Main.Controller
{
    public abstract class Controller : MonoBehaviour
    {
        private Coroutine _coroutine;

        private void OnEnable()
        {
            if (_coroutine != null) return;
            _coroutine = StartCoroutine(SubscribeOnceReady());
        }

        private IEnumerator SubscribeOnceReady()
        {
            yield return new WaitUntil(() => InputManager.Instance != null);
            _coroutine = null;
            Subscribe();
        }

        protected abstract void Subscribe();
    }
}