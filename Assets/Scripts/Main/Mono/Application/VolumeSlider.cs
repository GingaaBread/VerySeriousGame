using Audio;
using UnityEngine;

namespace Main.Mono.Application
{
    public class VolumeSlider : MonoBehaviour
    {
        public void UpdateSoundValue(float val)
        {
            AudioManager.Instance.SoundsVolume(val);
        }

        public void UpdateMusicValue(float val)
        {
            AudioManager.Instance.MusicVolume(val);
        }
    }
}