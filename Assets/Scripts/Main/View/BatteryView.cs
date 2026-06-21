using Main.Service;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Main.View
{
    public class BatteryView : MonoBehaviour
    {
        [Required] [SerializeField] private TMP_Text _amountText;
        [Required] [SerializeField] private Image _batterFillImage;
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
            _batterFillImage.fillAmount = (float)current / max;
        }
    }
}