using UnityEngine;

namespace Utility
{
    public class DoNotDestroyOnLoad : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}