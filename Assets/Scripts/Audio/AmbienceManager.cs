using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using FMODUnity;
using Audio;

public class AmbienceManager : MonoBehaviour
{
    private EventInstance _ambienceInstance;
    private EventInstance _reverbInstance;
    private string _currentScene;
    [SerializeField] private EventReference _mineReverbSnapshot;

    private void Start()
    {
        // Start the ambience loop
        _ambienceInstance = AudioManager.Instance.PlayLoop(AudioRegistry.Events.AmbienceHandler);
        _reverbInstance = RuntimeManager.CreateInstance(_mineReverbSnapshot);
        
        // Subscribe to scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
        
        // Set initial state
        UpdateAmbience(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateAmbience(scene.name);
    }

    private void UpdateAmbience(string sceneName)
    {
        bool isSurface = (sceneName == "Surface");
        float state = isSurface ? 1f : 0f;
        
        _ambienceInstance.setParameterByName("WorldState", state);
        
        if (!isSurface)
        {
            _reverbInstance.start();
        }
        else
        {
            _reverbInstance.stop(STOP_MODE.ALLOWFADEOUT);
        }

        Debug.Log($"Ambience set to {sceneName} (State: {state})");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _ambienceInstance.stop(STOP_MODE.ALLOWFADEOUT);
        _ambienceInstance.release();
    }
}
