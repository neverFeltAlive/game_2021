using System;
using UnityEngine;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// TriggerableTrack is used for extanding simple track mechinics with triggering option 
    /// (you need to first trigger saving before you track)
    /// :NeverFeltAlive
    /// 
    /// </summary>
    public class TriggerableTrack : Track
    {
        public event EventHandler<OnStateChangedEventArgs> OnStateChanged;



        #region Fields
        [SerializeField] [Range(15f, 100f)] [Tooltip("Cooldown time in seconds")] private float coolldownTime = 20f;

        private State state;

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
        }
        #endregion

        #region MonoBehaviour Callbacks
        protected override void Awake()
        {
            base.Awake();

            currentCooldownTime = coolldownTime;
            currentSavingTime = savingTime;
            state = State.Ready;
        }

        protected override void FixedUpdate()
        {
            if (state == State.Active)
                base.FixedUpdate();

            if (state == State.OnCooldown)
                HandleCooldown();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Counts down cooldown time
        /// </summary>
        private void HandleCooldown()
        {
            if (currentCooldownTime - Time.fixedDeltaTime > 0)
                currentCooldownTime -= Time.fixedDeltaTime;
            else
            {
                state = State.Ready;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                currentCooldownTime = coolldownTime;
            }
        }

        /// <summary>
        /// Saves the coordinates and counts down the time track is active
        /// </summary>
        protected override void HandleSaving()
        {
            if (currentSavingTime - Time.fixedDeltaTime > 0)
            {
                currentSavingTime -= Time.fixedDeltaTime;
                base.HandleSaving();
            }
            else
            {
                state = State.OnCooldown;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            }
        }

        public override void TriggerTrack()
        {
            if (state == State.Active) 
                base.TriggerTrack();

            if (state != State.OnCooldown)
            {
                state = State.OnCooldown;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
            }
        }

        /// <summary>
        /// Starts saving coordinates
        /// </summary>
        public void TriggerSaving()
        {
            if (state == State.Ready)
            {
                state = State.Active;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
                savedCoordinates = new Vector3[(int)(trackingTime / Time.fixedDeltaTime)];
                currentSavingTime = savingTime;
            }
        }
        #endregion
    }
}
