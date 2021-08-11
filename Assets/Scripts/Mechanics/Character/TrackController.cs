/// <remarks>
/// 
/// TrackController is used for controlling character's track mechanics
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

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
     */
    {
        public enum TrackingState
        {
            Ready,
            Active,
            Off,
            OnCooldown 
        }



        public static event EventHandler<OnTrackStateChangedEventArgs> OnTrackStateChanged;
        public class OnTrackStateChangedEventArgs : EventArgs
        {
            public TrackingState state;
        }
        public static event EventHandler<OnTrackEventArgs> OnTrack;
        public class OnTrackEventArgs : EventArgs
        {
            public float castingTime;
        }



        #region Serialized Fields
        [SerializeField] [Range(15f, 100f)] [Tooltip("Cooldown time in seconds")] private float coolldownTime = 20f;
        [SerializeField] [Range(0f, 7f)] [Tooltip("Time to which character travels back in seconds")] private float trackingTime = 3f;
        [SerializeField] [Tooltip("Time to which character travels back in seconds")] private float castingTime = 1f;
        #endregion

        #region Private Fields
        private Vector2[] savedCoordinates;             

        private TrackingState state;

        private float savingTime = 10f;
        private float currentSavingTime;
        private float currentCooldownTime;
        #endregion

        private bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>


        #region Context Menu
        [ContextMenu("Default values")]
        private void DefaultValues()
        {
            coolldownTime = 20f;
            trackingTime = 3f;
            castingTime = 1f;
        }
        #endregion

        #region MonoBehaviour Callbacks
        private void Start()
        {
            currentCooldownTime = coolldownTime;
            currentSavingTime = savingTime;
            state = TrackingState.Off;

            if (showDebug) OnTrackStateChanged += ShowState;
        }

        private void FixedUpdate()
        {
            if (state == TrackingState.Active || state == TrackingState.Ready)
                HandleSaving();

            if (state == TrackingState.OnCooldown)
                HandleCooldown();
        }
        #endregion

        #region Functions
        // Counts down cooldown time
        private void HandleCooldown()
        {
            if (currentCooldownTime - Time.fixedDeltaTime > 0)
                currentCooldownTime -= Time.fixedDeltaTime;
            else
            {
                state = TrackingState.Off;
                OnTrackStateChanged?.Invoke(this, new OnTrackStateChangedEventArgs { state = state });
                currentCooldownTime = coolldownTime;
            }
        }
        
        // Saves the coordinates and counts down the time track is active
        private void HandleSaving()
        {
            if (currentSavingTime - Time.fixedDeltaTime > 0)
            {
                currentSavingTime -= Time.fixedDeltaTime;

                // Check if the array is filled for the first time
                if (currentSavingTime <= savingTime - trackingTime && state != TrackingState.Ready)
                {
                    state = TrackingState.Ready;
                    OnTrackStateChanged?.Invoke(this, new OnTrackStateChangedEventArgs { state = state });
                }

                for (int j = savedCoordinates.Length - 1; j > 0; j--)
                    savedCoordinates[j] = savedCoordinates[j - 1];
                savedCoordinates[0] = transform.position;
                /// <summary>
                /// Its basicaly a stack-like system (last in - first out). Every selected interval a new elemnt is added which will be taken when the whole time period is over
                /// If there will be problems with optimization we can return intrval system (save coordinates not every fixed update but a much bigger time period
                /// </summary>

                if (showDebug) UtilsClass.DrawCross(transform.position, Color.white, 5f);
            }
            else
            {
                state = TrackingState.OnCooldown;
                OnTrackStateChanged?.Invoke(this, new OnTrackStateChangedEventArgs { state = state });
            }
        }

        private void ShowState(object sender, OnTrackStateChangedEventArgs args) =>
            Debug.Log("<size=13><i><b> TrackController --> </b></i><color=green> ShowState: </color></size>" + args.state);

        // Performs the actual track (is triggered by Player Input)
        public void Track(InputAction.CallbackContext context)
        {
            if (!context.performed)
                return;

            if (state == TrackingState.Ready)
            {
                state = TrackingState.OnCooldown;
                OnTrackStateChanged?.Invoke(this, new OnTrackStateChangedEventArgs { state = state });
                OnTrack?.Invoke(this, new OnTrackEventArgs { castingTime = castingTime });
                StartCoroutine(TrackCoroutine());
            }
        }

/*        // Triggers the saving cycle (is triggered by Player Input)
        public void TriggerTracking(InputAction.CallbackContext context)
        {

            if (!context.performed)
                return;

            if (state == TrackingState.Off)
            {
                state = TrackingState.Active;
                OnTrackStateChanged?.Invoke(this, new OnTrackStateChangedEventArgs { state = state });
                savedCoordinates = new Vector2[(int)(trackingTime / Time.fixedDeltaTime)];
                currentSavingTime = savingTime;
            }
        }
*/
        // Binds both action to one button
        public void TriggerAndTrack(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                if (state == TrackingState.Off)
                {
                    state = TrackingState.Active;
                    OnTrackStateChanged?.Invoke(this, new OnTrackStateChangedEventArgs { state = state });
                    savedCoordinates = new Vector2[(int)(trackingTime / Time.fixedDeltaTime)];
                    currentSavingTime = savingTime;
                }
            }

            if (context.canceled)
            {
                state = TrackingState.OnCooldown;
                OnTrackStateChanged?.Invoke(this, new OnTrackStateChangedEventArgs { state = state });
                OnTrack?.Invoke(this, new OnTrackEventArgs { castingTime = castingTime });
                StartCoroutine(TrackCoroutine());
            }
        }
        #endregion



        IEnumerator TrackCoroutine()
        {
            yield return new WaitForSeconds(castingTime);

            if (savedCoordinates[savedCoordinates.Length - 1] != default(Vector2))
                gameObject.transform.position = savedCoordinates[savedCoordinates.Length - 1];
            else
            {
                for (int i = savedCoordinates.Length - 1; i >= 0; i--)
                {
                    if (savedCoordinates[i] != default(Vector2))
                    {
                        transform.position = savedCoordinates[i];
                        break;
                    }
                    else if (i == 0)
                        transform.position = savedCoordinates[i];
                }
            }
        }
    }
}
