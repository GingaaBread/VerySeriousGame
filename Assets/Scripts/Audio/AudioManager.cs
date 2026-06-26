using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Volumes")]
        [Range(0f, 1f)] [SerializeField] private float masterVolume = 1f;

        private EventInstance _currentMusicInstance;

        // FMOD Mixing Board Bus Channels
        private Bus _masterBus;
        private Bus _musicBus;

        private float _musicVolume = 1f;
        private Bus _sfxBus;
        private float _sfxVolume = 1f;
        public static AudioManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            _masterBus = RuntimeManager.GetBus("bus:/");
            _musicBus = RuntimeManager.GetBus("bus:/Music");
            _sfxBus = RuntimeManager.GetBus("bus:/SFX");

            Debug.Log($"Music Bus Valid: {_musicBus.isValid()}");
            Debug.Log($"SFX Bus Valid: {_sfxBus.isValid()}");

            if (_masterBus.isValid()) _masterBus.setVolume(masterVolume);
        }

        private void OnDestroy()
        {
            StopMusic();
        }

        public void MusicVolume(float volume)
        {
            _musicVolume = volume;
            if (_musicBus.isValid()) _musicBus.setVolume(_musicVolume);
        }

        public void SoundsVolume(float volume)
        {
            _sfxVolume = volume;
            if (_sfxBus.isValid()) _sfxBus.setVolume(_sfxVolume);
        }

        //Plays any FMOD music event globally, automatically stopping and cleaning up the previous track.
        public void PlayMusic(EventReference musicEvent)
        {
            if (musicEvent.IsNull)
            {
                Debug.LogWarning("AudioManager: Intended music event is Null!");
                return;
            }

            StopMusic();

            _currentMusicInstance = RuntimeManager.CreateInstance(musicEvent);
            _currentMusicInstance.start();
            _currentMusicInstance.release();
        }

        // Stops the currently playing music track with a clean FMOD fadeout.
        public void StopMusic()
        {
            if (_currentMusicInstance.isValid())
                // ALLOWFADEOUT respects the AHDSR modulation you set up inside FMOD Studio!
                _currentMusicInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }

        // Method to play one shots in 2D space (UI, Ambience, FX, etc.)
        public void PlayOneShot(EventReference oneShotEvent)
        {
            if (!oneShotEvent.IsNull) RuntimeManager.PlayOneShot(oneShotEvent);
        }

        //Method to play one shots in 3D space (sounds coming from objects in space)
        public void PlayOneShot3D(EventReference oneShotEvent, Vector3 worldPosition)
        {
            if (!oneShotEvent.IsNull) RuntimeManager.PlayOneShot(oneShotEvent, worldPosition);
        }

        public EventInstance PlayLoop(EventReference loopEvent)
        {
            var instance = RuntimeManager.CreateInstance(loopEvent);
            instance.start();
            instance.release();
            return instance;
        }
    }
}