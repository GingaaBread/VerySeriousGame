using System.Collections;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.AddressableAssets;

// ReSharper disable Unity.InefficientPropertyAccess

namespace Utility.Polish
{
    /// <summary>
    ///     Allows an object to apply a flash material for a set amount of seconds.
    /// </summary>
    public class HitFlash : MonoBehaviour
    {
        private static Material _flashMaterial;

        [InfoBox("Determines for how long the flash sprite is applied (in seconds)")] [SerializeField]
        private float _flashTimeInSeconds = 0.15f;

        /// <summary>
        ///     Stores the default material of the game object that will be set again after applying the flash material.
        /// </summary>
        private Material _defaultMaterial;

        /// <summary>
        ///     Tracks the current routine of the applied flash effect, to immediately stop it when the flash is
        ///     re-applied.
        /// </summary>
        private Coroutine _flashRoutine;

        /// <summary>
        ///     Stores the sprite renderer of the object in order to set its material.
        /// </summary>
        private SpriteRenderer _spriteRenderer;

        /// <summary>
        ///     Stores the sprite renderer and default material of the object
        /// </summary>
        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _defaultMaterial = _spriteRenderer.material;
        }

        private async void Start()
        {
            if (_flashMaterial != null) return;
            _flashMaterial = await Addressables.LoadAssetAsync<Material>("material.flash").Task;
        }

        /// <summary>
        ///     Stops the current flash routine if it exists and starts a new one.
        /// </summary>
        public void Flash()
        {
            if (_flashRoutine is not null) StopCoroutine(_flashRoutine);
            _flashRoutine = StartCoroutine(StartFlash());
        }

        /// <summary>
        ///     Applies the flash material, waits for the specified time, and applies the default material.
        /// </summary>
        private IEnumerator StartFlash()
        {
            _spriteRenderer.material = _flashMaterial;
            yield return new WaitForSecondsRealtime(_flashTimeInSeconds);
            _spriteRenderer.material = _defaultMaterial;
        }
    }
}