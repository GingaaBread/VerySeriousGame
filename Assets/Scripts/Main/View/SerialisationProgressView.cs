using System.Collections;
using Main.Service;
using TMPro;
using UnityEngine;
using VContainer;

namespace Main.View
{
    public class SerialisationProgressView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _progressText;
        [Inject] private readonly SerialisationService _serialisationService;

        private void OnEnable()
        {
            StartCoroutine(WaitForService());
        }

        private void OnDisable()
        {
            _serialisationService.OnProgressUpdate -= Render;
        }

        private IEnumerator WaitForService()
        {
            yield return new WaitUntil(() => _serialisationService != null);
            _serialisationService.OnProgressUpdate += Render;
            var progress = _serialisationService.GetCurrentProgress();
            Render(progress.Item1, progress.Item2);
        }

        private void Render(int destroyedAmount, int totalAmount)
        {
            _progressText.text = destroyedAmount + "/" + totalAmount;
        }
    }
}