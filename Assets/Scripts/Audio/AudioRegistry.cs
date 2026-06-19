using UnityEngine;
using FMODUnity;

public class AudioRegistry : MonoBehaviour
{
    public static AudioRegistry Events;

    [Header("Music")]
    public EventReference musicTest;

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


