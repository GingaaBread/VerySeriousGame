using System.Collections;
using Main.Mono.Collected_Items;
using UnityEngine;

namespace Utility.Polish
{
    public class HitSlash : MonoBehaviour
    {
        private const float SAFETY_COOLDOWN = 0.1f;
        private bool _canFire = true;

        private IEnumerator Cooldown()
        {
            _canFire = false;
            yield return new WaitForSeconds(SAFETY_COOLDOWN);
            _canFire = true;
        }

        public void Slash()
        {
            if (!_canFire) return;
            StartCoroutine(Cooldown());
            SlashManager.Instance.RequireAt(transform.position);
        }
    }
}