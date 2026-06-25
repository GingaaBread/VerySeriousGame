using System.Collections;
using Main.Service;
using UnityEngine;
using VContainer;

namespace Main.Mono.Application
{
    public class DrillStop : MonoBehaviour
    {
        [Inject] private readonly DrillService _drillService;

        private void Start()
        {
            StartCoroutine(WaitForService());
        }

        private IEnumerator WaitForService()
        {
            yield return new WaitUntil(() => _drillService != null);
            if (_drillService.IsActivated()) _drillService.ToggleActivity();
        }
    }
}