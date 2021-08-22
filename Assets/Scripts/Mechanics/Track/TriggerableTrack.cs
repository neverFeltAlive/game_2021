/// <remarks>
/// 
/// TriggerableTrack is used for extanding simple track mechinics with triggering option 
/// (you need to first trigger saving before you track)
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using UnityEngine;

namespace Custom.Mechanics
{
    public class TriggerableTrack : Track
    /* DEBUG statements for this document 
     * 
     * Debug.Log("TriggerableTrack --> Start: ");
     * Debug.Log("<size=13><i><b> TriggerableTrack --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> TriggerableTrack --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> TriggerableTrack --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> TriggerableTrack --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public enum TrackingState
        {
            Active,
            Off,
            OnCooldown
        }



        public static event EventHandler<OnTrackStateChangedEventArgs> OnTrackStateChanged;
        public class OnTrackStateChangedEventArgs : EventArgs
        {
            public TrackingState state;
        }



        #region Serialized Fields
        [SerializeField] [Range(15f, 100f)] [Tooltip("Cooldown time in seconds")] private float coolldownTime = 20f;
        [Tooltip("Time to which character travels back in seconds")] public float castingTime = 1f;
        #endregion

        #region Private Fields
        private TrackingState state;

        private float savingTime = 10f;
        private float currentSavingTime;
        private float currentCooldownTime;
        #endregion



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
        protected override void Awake()
        {
            base.Awake();

            currentCooldownTime = coolldownTime;
            currentSavingTime = savingTime;
            state = TrackingState.Off;
        }

        protected override void FixedUpdate()
        {
            if (state == TrackingState.Active)
                base.FixedUpdate();

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
        protected override void HandleSaving()
        {
            if (currentSavingTime - Time.fixedDeltaTime > 0)
            {
                currentSavingTime -= Time.fixedDeltaTime;
                base.HandleSaving();
            }
            else
            {
                state = TrackingState.OnCooldown;
                OnTrackStateChanged?.Invoke(this, new OnTrackStateChangedEventArgs { state = state });
            }
        }

        public override void TriggerTrack()
        {
            if (state == TrackingState.Active) 
            {
                base.TriggerTrack();
            }

            if (state != TrackingState.OnCooldown)
            {
                state = TrackingState.OnCooldown;
                OnTrackStateChanged?.Invoke(this, new OnTrackStateChangedEventArgs { state = state });
            }
        }

        public void TriggerSaving()
        {
            if (state == TrackingState.Off)
            {
                state = TrackingState.Active;
                OnTrackStateChanged?.Invoke(this, new OnTrackStateChangedEventArgs { state = state });
                savedCoordinates = new Vector3[(int)(trackingTime / Time.fixedDeltaTime)];
                currentSavingTime = savingTime;
            }
        }
        #endregion
    }
}
