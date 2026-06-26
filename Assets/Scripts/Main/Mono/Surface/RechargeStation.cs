using Main.Service;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace Main.Mono.Surface
{
    public class RechargeStation : MonoBehaviour
    {
        [SerializeField] private UnityEvent _onRecharge;
        [Inject] private readonly BatteryService _batteryService;

        public void Recharge()
        {
            _batteryService.RechargeCompletely();
            _onRecharge?.Invoke();
        }
    }
}