using System.Collections;
using Main.Mono.Surface;
using UnityEngine;

namespace Main.Mono.Player
{
    public class TeleportationManager : MonoBehaviour
    {
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private SceneChange _change;
        [SerializeField] private Animator _postProcessingAnimator;

        private bool _isTeleporting;

        public void Teleport()
        {
            if (_isTeleporting) return;

            _isTeleporting = true;

            _playerMovement.Freeze();
            _postProcessingAnimator.Play("Teleport");
            StartCoroutine(LoadAfterTime());
        }

        private IEnumerator LoadAfterTime()
        {
            yield return new WaitForSeconds(0.7f);
            _change.Load();
        }
    }
}