using UnityEngine;
using UnityEngine.SceneManagement;
using FMOD.Studio;
using Audio;

public class AmbienceManager : MonoBehaviour
{
    private EventInstance _ambienceInstance;
    private string _currentScene;

    private void Start()
    {
        // Start the ambience loop
        _ambienceInstance = AudioManager.Instance.PlayLoop(AudioRegistry.Events.WorldAmbience);
        
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
        // Assuming your scene names are "Surface" and "Mines"
        float state = (sceneName == "Surface") ? 1f : 0f;
        
        _ambienceInstance.setParameterByName("WorldState", state);
        Debug.Log($"Ambience set to {sceneName} (State: {state})");
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _ambienceInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _ambienceInstance.release();
    }
}
