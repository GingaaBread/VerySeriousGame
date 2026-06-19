using UnityEngine;
using UnityEngine.SceneManagement;

namespace Main.Mono.Player
{
    public class TargetReachTracker : MonoBehaviour
    {
        [SerializeField] private Transform _centreTransform;

        private void Update()
        {
            if (transform.position.y > _centreTransform.position.y) return;
            WinGame();
        }

        private void WinGame()
        {
            Debug.Log("Well done! Game completed");
            SceneManager.LoadScene("Main");
        }
    }
}