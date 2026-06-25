using UnityEngine;

namespace Utility
{
    public class BaseScope : MonoBehaviour
    {
        private static bool _awoken;

        private void Awake()
        {
            if (_awoken)
            {
                Destroy(gameObject);
                return;
            }

            _awoken = true;
            DontDestroyOnLoad(gameObject);
        }
    }
}