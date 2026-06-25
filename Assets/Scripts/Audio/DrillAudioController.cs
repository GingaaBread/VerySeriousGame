using System.Collections;
using FMOD.Studio;
using Main.Service;
using UnityEngine;
using VContainer;

namespace Audio
{
    public class DrillAudioController : MonoBehaviour
    {
        [Inject] private readonly DrillActivationMediator _drillActivationMediator;
        private EventInstance _drillInstance;

        private void OnEnable()
        {
            StartCoroutine(WaitForService());
        }

        private void OnDisable()
        {
            _drillActivationMediator.OnActivationChange -= UpdateFmodDrill;
            _drillInstance.stop(STOP_MODE.IMMEDIATE);
            _drillInstance.release();
        }

        private IEnumerator WaitForService()
        {
            yield return new WaitUntil(() => _drillActivationMediator != null);
            _drillActivationMediator.OnActivationChange += UpdateFmodDrill;
        }

        private void UpdateFmodDrill(bool drillIsActive)
        {
            if (drillIsActive)
            {
                _drillInstance = AudioManager.Instance.PlayLoop(AudioRegistry.Events.DrillDrilling);
                _drillInstance.setParameterByName("isDrilling", 1f);
                Debug.Log("Forced Drill ON");
            }
            else
            {
                if (_drillInstance.isValid()) _drillInstance.setParameterByName("isDrilling", 0f);
            }
        }
    }
}