using Main.Service;
using TMPro;
using UnityEngine;
using VContainer;

namespace Main.View
{
    public class BatteryView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountText;
        [Inject] private readonly BatteryService _batteryService;

        private void OnEnable()
        {
            _batteryService.OnBatteryAmountUpdated += Render;
            Render(_batteryService.GetCurrentBattery(), _batteryService.GetCurrentMaxBattery());
        }

        private void OnDisable()
        {
            _batteryService.OnBatteryAmountUpdated -= Render;
        }


        private void Render(int current, int max)
        {
            _amountText.text = current + "/" + max;
        }
    }
}