/// <remarks>
/// 
/// TrackController is used for controlling character's track mechanics
/// NeverFeltAlive
/// 
/// </remarks>


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Platformer.Utils;

namespace Platformer.Mechanics.Character
{
    public class TrackController : MonoBehaviour
    /* DEBUG statements for this document 
     * 
     * Debug.Log("TrackController --> Start: ");
     * Debug.Log("<size=13><i><b> TrackController --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> TrackController --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> TrackController --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> TrackController --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     * Make triggerable
     * Think about moving saved coordinates to Heap with collections
     * 
     */
    {
        // Fields

        #region Serialized Fields
        [Space]
        [Space]
        [Header("TrackController Script")]
        [Space]
        [Header("Components")]
        [SerializeField] private CharacterMovementController movement;
        [Space]
        [Header("Variables")]
        [SerializeField] [Range(0f, 5f)] [Tooltip("Movement stop time after track")] private float stopTime = 0.25f;
        [Space]
        [SerializeField] [Range(0f, 10f)] [Tooltip("Cooldown time in seconds")] private float coolldownTime = 1f;
        [Space]
        [SerializeField] [Range(0f, 80f)] [Tooltip("Time to which character travels back in seconds")] private float trackingTime = 10f;
        [SerializeField] [Range(0, 1)] [Tooltip("Coordinats saving interval in fractions of a second (by default is equal to fixed delta time)")] private float interval = 0.25f;
        #endregion

        #region Private Fields
        private Vector2[] savedCoordinates;             // save coordinates

        private int numberOfIntervals;                  // number of intervals in tracking time (number of saved coordinates in array)

        private bool onCooldown = false;                // flag for track cooldownTime
        private bool isSaving = false;                  // flag for saving coroutine
        private bool triggerTrack = false;              // trigger track
        #endregion

        #region Properties
        #endregion

        // Functions 

        #region Context Menu
        [ContextMenu("Default values")]
        private void DefaultValues()
        {
            coolldownTime = 1f;
            trackingTime = 3f;
            interval = 0.25f;
            stopTime = 0.25f;
        }
        #endregion

        #region MonoBehaviour Callbacks
        // When the script is enabled
        private void Start()
        {
            // Assign variables
            if (interval == 0)
                interval = Time.fixedDeltaTime;
            if (!movement)
                movement = gameObject.GetComponent<CharacterMovementController>();

            numberOfIntervals = (int)(trackingTime * 1 / interval);
            savedCoordinates = new Vector2[numberOfIntervals];
        }

        // Every frame
        private void Update()
        {
            // Trigger time travel
            if (Input.GetButtonDown(Constants.X_BUTTON) && !onCooldown)
                triggerTrack = true;

            // Restart saving coroutine
            if (!isSaving)
                StartCoroutine(Save());
        }

        private void FixedUpdate()
        {
            // Move character
            if (triggerTrack)
            {
                gameObject.transform.position = savedCoordinates[savedCoordinates.Length - 1];

                // Start cooldownTime
                StartCoroutine(Cooldown());

                // Stop movement
                movement.StopMoving(stopTime);
                /// <remarks>
                /// Stopping movement is needed for the same purpose as in dash mechanics
                /// </remarks>

                // Manage flags 
                triggerTrack = false;
            }
        }
        #endregion

        #region Private Functions
        #endregion

        #region Event Handlers
        #endregion

        // Methods

        #region Private Methods
        #endregion

        // Coroutines

        #region IEnumerators
        // Saving coroutin 
        IEnumerator Save()
        {
            // Manage flag
            isSaving = true;

            for (int i = 0; i < savedCoordinates.Length; i++)
            {
                // Move all elements 1 index up
                for (int j = savedCoordinates.Length - 1; j > 0; j--)
                    savedCoordinates[j] = savedCoordinates[j - 1];

                // Save coordinates to the first element
                savedCoordinates[0] = gameObject.transform.position;

                // Wait for the next saving interval
                yield return new WaitForSeconds(interval);
            }
            /// <summary>
            /// Its basicaly a stack-like system (last in - first out). Every selected interval a new elemnt is added which will be taken when the whole time period is over
            /// </summary>

            // DEBUG
            Debug.Log("<size=13><i><b> TrackController --> </b></i><color=blue> Corutine: </color></size>Saving cycle finished ");

            // Manage flags
            isSaving = false;
        }

        // Cooldown coroutine
        IEnumerator Cooldown()
        {
            // Manage flags
            onCooldown = true;

            // DEBUG
            Debug.Log("<size=13><i><b> TrackController --> </b></i><color=blue> Corutine: </color></size> Track on cooldown");

            // Wait for cooldownTime
            yield return new WaitForSeconds(coolldownTime);

            // DEBUG
            Debug.Log("<size=13><i><b> TrackController --> </b></i><color=blue> Corutine: </color></size> Track ready");

            // Manage flags
            onCooldown = false; 
        }
        #endregion

    }
}
