using UnityEngine;
using UnityEngine.Events;

namespace Main.Mono.Lore
{
    public class SecondCutsceneTrigger : MonoBehaviour
    {
        private static bool _hasTriggered;
        [SerializeField] private UnityEvent _onTrigger;

        public void Trigger()
        {
            if (_hasTriggered) return;
            _hasTriggered = true;
            _onTrigger?.Invoke();
        }
    }
}