using FMODUnity;
using UnityEngine;

namespace Audio
{
    public class AudioRegistry : MonoBehaviour
    {
        public static AudioRegistry Events;


        [Header("Ambience")]
        public EventReference AmbienceHandler;

        [Header("SFX")]
        public EventReference DrillDrilling;

        public EventReference DrillHitDirt;
        public EventReference DrillHitResource;
        public EventReference DrillDestroyResource;
        public EventReference ItemPickup;
        public EventReference StoreBuy;
        public EventReference StoreError;
        public EventReference StoreTab;
        public EventReference StoreOpen;
        public EventReference Footsteps;
        public EventReference Transition;
        public EventReference Recharge;
        public EventReference VoiceLines;
        public EventReference WorldMusic;
        public EventReference Dialogue;


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
}