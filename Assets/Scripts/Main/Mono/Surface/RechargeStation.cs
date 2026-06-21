using Main.Service;
using UnityEngine;
using VContainer;

namespace Main.Mono.Surface
{
    public class RechargeStation : MonoBehaviour
    {
        [Inject] private readonly BatteryService _batteryService;

        public void Recharge()
        {
            _batteryService.RechargeCompletely();
        }
    }
}