using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utility.Transitions;

namespace Main.Mono.Surface
{
    public class SceneChange : MonoBehaviour
    {
        [Scene] [SerializeField] private string _sceneName;
        [Required] [SerializeField] private Transition _transition;

        private TransitionEmitter.EmitterEvents _emitterEvents;

        private void Awake()
        {
            _emitterEvents = new();
            _emitterEvents.OnHoldStart.AddListener(LoadScene);
        }

        private void LoadScene()
        {
            SceneManager.LoadScene(_sceneName);
        }

        public void Load()
        {
            TransitionManager.Instance.Receive(_transition, _emitterEvents);
        }
    }
}