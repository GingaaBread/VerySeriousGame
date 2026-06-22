using UnityEngine;
using FMODUnity;

public class AudioRegistry : MonoBehaviour
{
    public static AudioRegistry Events;

    [Header("Music")]
    public EventReference musicTest;
    
    [Header("SFX")]
    public EventReference drillDrilling;
    public EventReference drillHitDirt;
    public EventReference drillHitResource;
    public EventReference drillDestroyResource;

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


