using UnityEngine;
using FMODUnity;

public class AudioRegistry : MonoBehaviour
{
    public static AudioRegistry Events;

    [Header("Music")]
    public EventReference MusicTest;
    
    [Header("SFX")]
    public EventReference DrillDrilling;
    public EventReference DrillHitDirt;
    public EventReference DrillHitResource;
    public EventReference DrillDestroyResource;

    private void Awake()
    {
        // Set up the static reference shortcut
        if (Events != null && Events != this)
        {
            Destroy(gameObject);
            return;
        }
        Events = this;
    }
}


