using System.Collections;
using NaughtyAttributes;
using UnityEngine;

namespace Utility.Polish
{
    /// <summary>
    ///     Allows objects to change the current time scale for a set amount of seconds.
    /// </summary>
    public class TimeChange : MonoBehaviour
    {
        [InfoBox("Determines the time scale in seconds that will be applied by this change")] [SerializeField]
        private float _newTimeInSeconds;

        [InfoBox("Determines the amount of seconds that the time will remain at the specified amount " +
                 "until it is set back to the prior time")]
        [SerializeField]
        private float _changeLastingTimeInSeconds;

        private float _lastTime;

        private void OnDisable()
        {
            Time.timeScale = _lastTime;
        }

        /// <summary>
        ///     Applies the amount of time, but only if the current time is higher than the desired one.
        ///     This convention is used to prevent time scale issues.
        /// </summary>
        public void ApplyChange()
        {
            if (Time.timeScale <= _newTimeInSeconds) return;
            StartCoroutine(ChangeTimeScale());
        }

        /// <summary>
        ///     Sets the desired time scale, waits for the specified amount of time, and then applies the prior
        ///     time scale again.
        /// </summary>
        private IEnumerator ChangeTimeScale()
        {
            _lastTime = Time.timeScale;
            Time.timeScale = _newTimeInSeconds;

            yield return new WaitForSecondsRealtime(_changeLastingTimeInSeconds);

            Time.timeScale = _lastTime;
        }
    }
}