using UnityEngine;
using Audio;

public class PlaySoundOnTrigger : MonoBehaviour
{
    // Drag this into the UnityEvent list in the Inspector
    public void PlayTransition()
    {
        AudioManager.Instance.PlayOneShot(AudioRegistry.Events.Transition);
    }

    public void PlayRecharge()
    {
        AudioManager.Instance.PlayOneShot(AudioRegistry.Events.Recharge);
    }
    
    public void PlayConfirm()
    {
        AudioManager.Instance.PlayOneShot(AudioRegistry.Events.StoreTab);
    }
}
