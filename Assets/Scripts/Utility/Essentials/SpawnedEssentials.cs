using UnityEngine;

namespace Utility.Essentials
{
    public class SpawnedEssentials : MonoBehaviour
    {
        [field: SerializeField] public GameObject[] Injected { private set; get; }

        public static bool Exists { private set; get; }

        private void Awake()
        {
            if (Exists) return;

            DontDestroyOnLoad(gameObject);
            Exists = true;
        }
    }
}