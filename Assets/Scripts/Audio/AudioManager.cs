using FMOD.Studio;
using FMODUnity;
using UnityEngine;

namespace Audio
{
    public class AudioManager : MonoBehaviour
    {
        [Header("Volumes")]
        [Range(0f, 1f)] [SerializeField] private float masterVolume = 1f;

        [Range(0f, 1f)] [SerializeField] private float musicVolume = 1f;
        [Range(0f, 1f)] [SerializeField] private float sfxVolume = 1f;
        [Range(0f, 1f)] [SerializeField] private float ambienceVolume = 1f;
        private Bus _ambienceBus;

        private EventInstance _currentMusicInstance;

        // FMOD Mixing Board Bus Channels
        private Bus _masterBus;
        private Bus _musicBus;
        private Bus _sfxBus;
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
            _ambienceBus = RuntimeManager.GetBus("bus:/Ambience");
        }

        private void Start()
        {
            if (AudioRegistry.Events != null && !AudioRegistry.Events.musicTest.IsNull)
                PlayMusic(AudioRegistry.Events.musicTest);
            else
                Debug.LogWarning("AudioManager: Could not find an In Game Theme event in the AudioRegistry!");
        }

        private void Update()
        {
            if (_masterBus.isValid()) _masterBus.setVolume(masterVolume);
            if (_musicBus.isValid()) _musicBus.setVolume(musicVolume);
            if (_sfxBus.isValid()) _sfxBus.setVolume(sfxVolume);
            if (_ambienceBus.isValid()) _ambienceBus.setVolume(ambienceVolume);
        }

        private void OnDestroy()
        {
            StopMusic();
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
    }
}