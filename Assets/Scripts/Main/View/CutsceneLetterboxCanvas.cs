using UnityEngine;

namespace Main.View
{
    public class CutsceneLetterboxCanvas : MonoBehaviour
    {
        private static readonly int OUT = Animator.StringToHash("Out");
        [SerializeField] private Animator _containerAnimator;
        public static CutsceneLetterboxCanvas Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        public void Toggle(bool show)
        {
            if (show)
            {
                _containerAnimator.gameObject.SetActive(true);
                _containerAnimator.Play("Letterbox_In");
            }
            else
            {
                _containerAnimator.SetTrigger(OUT);
            }
        }
    }
}