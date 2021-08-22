/// <remarks>
/// 
/// Track is used for controlling simple track mechanics
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    public class Track : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("Track --> Start: ");
     * Debug.Log("<size=13><i><b> Track --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> Track --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> Track --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> Track --> </b></i><color=green> Function: </color></size>");
     * 
     */
    {
        public static event EventHandler OnTrack;



        [SerializeField] [Range(1f, 5f)] [Tooltip("Time to which character travels back in seconds")] protected float trackingTime = 3f;

        protected Vector3[] savedCoordinates;

        protected bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>



        protected virtual void Awake() =>
            savedCoordinates = new Vector3[(int)(trackingTime / Time.fixedDeltaTime)];

        protected virtual void FixedUpdate() =>
            HandleSaving();

        // Saves current coordinates
        protected virtual void HandleSaving()
        {
            for (int j = savedCoordinates.Length - 1; j > 0; j--)
                savedCoordinates[j] = savedCoordinates[j - 1];
            savedCoordinates[0] = transform.position;
            /// <summary>
            /// Its basicaly a stack-like system (last in - first out). Every selected interval a new elemnt is added which will be taken when the whole time period is over
            /// If there will be problems with optimization we can return intrval system (save coordinates not every fixed update but a much bigger time period
            /// </summary>

            // DEBUG
            if (showDebug) UtilsClass.DrawCross(transform.position, Color.white, 5f);
        }

        // Performs track
        public virtual void TriggerTrack()
        {
            // Check if any coordinates are saved
            if (GetLastSavedCoordinates() != default(Vector3))
            {
                OnTrack?.Invoke(this, EventArgs.Empty);
                transform.position = GetLastSavedCoordinates();
            }

            savedCoordinates = new Vector3[(int)(trackingTime / Time.fixedDeltaTime)];
        }

        // Retrieves last saved coordinates from the list
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
    }
}
