using UnityEngine;
using Audio;

public class FootstepBridge : MonoBehaviour
{
    public void PlayFootstepSound()
    {
        AudioManager.Instance.PlayOneShot(AudioRegistry.Events.Footsteps);
    }
}
