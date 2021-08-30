using System;
using System.Collections;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// Track is used for controlling simple track mechanics
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(CharacterMovement))]
    public class Track : MonoBehaviour
    {
        public event EventHandler OnTrack;



        #region Fields
        [SerializeField] [Range(1f, 5f)] [Tooltip("Time to which character travels back in seconds")] protected float trackingTime = 3f;

        protected Vector3[] savedCoordinates;
        #endregion

        #region DEBUG
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>
        protected bool showDebug = true;
        #endregion



        protected virtual void Awake() =>
            savedCoordinates = new Vector3[(int)(trackingTime / Time.fixedDeltaTime)];

        protected virtual void FixedUpdate() =>
            HandleSaving();

        #region Mechanics Overrides
        public virtual void TriggerTrack()
        {
            // Check if any coordinates are saved
            if (GetLastSavedCoordinates() != default(Vector3))
                StartCoroutine(TrackCoroutine());
        }
        #endregion

        /// <summary>
        /// Saves current coordinates
        /// </summary>
        protected virtual void HandleSaving()
        {
            for (int j = savedCoordinates.Length - 1; j > 0; j--)
                savedCoordinates[j] = savedCoordinates[j - 1];
            savedCoordinates[0] = transform.position;
            /// Its basicaly a stack-like system (last in - first out). Every selected interval a new elemnt is added which will be taken when the whole time period is over
            /// If there will be problems with optimization we can return intrval system (save coordinates not every fixed update but a much bigger time period

            #region DEBUG
            if (showDebug) UtilsClass.DrawCross(transform.position, Color.white, 5f);
            #endregion
        }

        /// <summary>
        /// Retrieves last saved coordinates from the list
        /// </summary>
        protected Vector3 GetLastSavedCoordinates()
        {
            Vector3 returnVector = default(Vector3);
            for (int i = savedCoordinates.Length - 1; i >= 0; i--)
            {
                if (savedCoordinates[i] != default(Vector3))
                {
                    returnVector = savedCoordinates[i];
                    break;
                }
            }

            return returnVector;
        }



        #region Coroutines
        protected IEnumerator TrackCoroutine()
        {
            float trackDelay = 1f;
            IDisablableMovement movement = GetComponent<CharacterMovement>();

            OnTrack?.Invoke(this, EventArgs.Empty);

            movement.DisableMovement();
            yield return new WaitForSeconds(trackDelay);

            transform.position = GetLastSavedCoordinates();
            savedCoordinates = new Vector3[(int)(trackingTime / Time.fixedDeltaTime)];

            yield return new WaitForSeconds(trackDelay);
            movement.EnableMovement();
        }
        #endregion
    }
}
