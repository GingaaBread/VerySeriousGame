using System.Collections;
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
            StartCoroutine(WaitForService());
        }

        private void OnDisable()
        {
            _batteryService.OnBatteryAmountUpdated -= Render;
        }

        private IEnumerator WaitForService()
        {
            yield return new WaitUntil(() => _batteryService != null);
            _batteryService.OnBatteryAmountUpdated += Render;
            Render(_batteryService.GetCurrentBattery(), _batteryService.GetCurrentMaxBattery());
        }


        private void Render(int current, int max)
        {
            _amountText.text = current + "/" + max;
            _batterFillImage.fillAmount = (float)current / max;
        }
    }
}