using UnityEngine;

namespace Main.Mono.Application
{
    public class ApplicationQuitter : MonoBehaviour
    {
        public void Quit()
        {
            Time.timeScale = 0f;
            UnityEngine.Application.Quit();
        }
    }
}