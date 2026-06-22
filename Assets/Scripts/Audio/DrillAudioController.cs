using UnityEngine;
using FMOD.Studio;
using VContainer;

namespace Audio
{
    public class DrillAudioController : MonoBehaviour
    {
        private EventInstance _drillInstance;

        [Inject] private readonly Main.Service.DrillActivationMediator _drillActivationMediator;

        private void OnEnable()
        {
            _drillActivationMediator.OnActivationChange += UpdateFmodDrill;
        }

        private void OnDisable()
        {
            _drillActivationMediator.OnActivationChange -= UpdateFmodDrill;
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
                if (_drillInstance.isValid())
                {
                    _drillInstance.setParameterByName("isDrilling", 0f);
                }
            }
        }
    }
}