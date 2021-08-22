/// <remarks>
/// 
/// DashAndReturn is used for extending dash mechanics
/// It adds a cooldown mechanics, a posibolity to return to the destinations of dash
/// And overload mechanics
/// NeverFeltAlive
/// 
/// </remarks>


using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    public class DashAndReturn : Dash
    /* DEBUG statements for this document 
     * 
     * Debug.Log("DashAndReturn --> Start: ");
     * Debug.Log("<size=13><i><b> DashAndReturn --> </b></i><color=yellow> FixedUpdate: </color></size>");
     * Debug.Log("<size=13><i><b> DashAndReturn --> </b></i><color=red> Update: </color></size>");
     * Debug.Log("<size=13><i><b> DashAndReturn --> </b></i><color=blue> Corutine: </color></size>");
     * Debug.Log("<size=13><i><b> DashAndReturn --> </b></i><color=green> Function: </color></size>");
     * 
     */
    /* TODO
     * 
     */
    {
        public enum DashState
        {
            Ready,
            Active,
            OnCooldown
        }
        public enum ReturnState
        {
            Active,
            InActive
        }



        public static event EventHandler<OnDashStateChangedEventArgs> OnDashStateChanged;
        public class OnDashStateChangedEventArgs : EventArgs
        {
            public bool isPower;
            public DashState state;
            public Vector3 direction;
        }
        public static event EventHandler<OnReturnStateChangedEventArgs> OnReturnStateChanged;
        public class OnReturnStateChangedEventArgs : EventArgs
        {
            public ReturnState state;
        }



        #region Serialized Fields
        [SerializeField] [Range(1, 5)] [Tooltip("Max number of dashed available at once")] private int maxNumberOfDashes = 5;
        [SerializeField] [Range(0f, 100f)] [Tooltip("Dashing force for overloaded dash")] private float overLoadMultiplier = 1f;
        [SerializeField] [Range(1, 5)] [Tooltip("Time dash remains inactive after being complete")] private float dashInterval = .3f;
        [Space] [Header("Cooldown settings")]
        [SerializeField] [Range(1f, 10f)] [Tooltip("Dash cooldown time in seconds")] private float cooldownTime = 3f;
        [SerializeField] [Range(1f, 10f)] [Tooltip("Dash cooldown time after every return in seconds")] private float returnCooldownTime = 2f;
        [SerializeField] [Range(0f, 10f)] [Tooltip("Dash cooldown time after last return in seconds")] private float lastReturnCooldownTime = .5f;
        [SerializeField] [Range(.1f, 5f)] [Tooltip("Time  after one dash untill coldown is triggered")] private float cooldownTriggeringTime = .8f;
        #endregion

        #region Private Fields
        private bool isOverLoaded;

        private float currentCooldownTriggeringTime;
        private float overLoadTime;

        private List<Vector3> savedCoordinates;

        private DashState dashState;
        private ReturnState returnState;
        #endregion

        private bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>



        #region Context Menu
        [ContextMenu("Default Values")]
        private void DefaultValues()
        {
            force = .45f;
            minMagnitude = .6f;

            maxNumberOfDashes = 5;
            overLoadMultiplier = 1f;
            dashInterval = .3f;

            cooldownTime = 3f;
            returnCooldownTime = 2f;
            lastReturnCooldownTime = .5f;
            cooldownTriggeringTime = .8f;
        }
        #endregion

        #region MonoBehaviour Callbacks
        protected override void Awake()
        {
            base.Awake();

            dashState = DashState.Ready;
            returnState = ReturnState.InActive;

            savedCoordinates = new List<Vector3>();
            currentCooldownTriggeringTime = cooldownTriggeringTime;
            isOverLoaded = false;
        }

        private void Update()
        {
            CheckCooldownTriggeringTime();
            CheckOverLoadTime();
        }
        #endregion

        #region Functions
        // Check if too much time passed since last dash
        private void CheckCooldownTriggeringTime()
        {
            if (dashState == DashState.Ready && savedCoordinates.Count != 0)
            {
                if (currentCooldownTriggeringTime - Time.deltaTime < 0)
                {
                    currentCooldownTriggeringTime = cooldownTriggeringTime;
                    StartCoroutine(Cooldown(cooldownTime));
                }
                else
                    currentCooldownTriggeringTime -= Time.deltaTime;
            }
            else
                currentCooldownTriggeringTime = cooldownTriggeringTime;
        }

        private void CheckOverLoadTime()
        {
            if (isOverLoaded)
            {
                if (overLoadTime - Time.deltaTime < 0)
                    isOverLoaded = false;
            }
        }

        public void TriggerDash(Vector3 direction, bool isOverLoaded = false)
        {
            if (dashState != DashState.Ready)
                return;

            if (this.isOverLoaded || isOverLoaded)
                StartCoroutine(DashCoroutine(direction, overLoadMultiplier));
            else
                StartCoroutine(DashCoroutine(direction));
        }

        public void HandleReturn()
        {
            if (returnState == ReturnState.Active)
            {
                gameObject.transform.position = savedCoordinates[savedCoordinates.Count - 1];
                savedCoordinates.RemoveAt(savedCoordinates.Count - 1);

                float cooldownTime;
                if (savedCoordinates.Count == 0)
                {
                    returnState = ReturnState.InActive;
                    OnReturnStateChanged?.Invoke(this, new OnReturnStateChangedEventArgs { state = returnState });
                    cooldownTime = lastReturnCooldownTime;
                }
                else
                    cooldownTime = returnCooldownTime;

                StopAllCoroutines();
                StartCoroutine(Cooldown(cooldownTime));
                /// <remarks>
                /// We need to stop all coroutines to make last return completely restart cooldown with new timer
                /// </remarks>
            }
        }

        public void TriggerOverLoad(float time)
        {
            overLoadTime = time;
            isOverLoaded = true;
        }
        #endregion



        #region Coroutines
        IEnumerator Cooldown(float time)
        {
            dashState = DashState.OnCooldown;
            OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs { state = dashState });

            yield return new WaitForSeconds(time); // wait for required amount of seconds

            savedCoordinates = new List<Vector3>();
            returnState = ReturnState.InActive;
            dashState = DashState.Ready;
            OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs { state = dashState });
            OnReturnStateChanged?.Invoke(this, new OnReturnStateChangedEventArgs { state = returnState });
        }

        IEnumerator DashCoroutine(Vector3 direction, float multiplier = 1)
        {
            dashState = DashState.Active;
            OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs { state = dashState });

            HandleDash(direction, multiplier);

            yield return new WaitForFixedUpdate();

            savedCoordinates.Add(transform.position);

            // DEBUG
            if (showDebug)
                UtilsClass.DrawCross(transform.position, Color.yellow, 2f);

            returnState = ReturnState.Active;
            OnReturnStateChanged?.Invoke(this, new OnReturnStateChangedEventArgs { state = returnState });

            yield return new WaitForSeconds(dashInterval);
            /// <remarks>
            /// Shoud be set to animation time
            /// </remarks>

            if (isOverLoaded && !this.isOverLoaded)
                StartCoroutine(Cooldown(cooldownTime));
            else
            {
                if (savedCoordinates.Count == maxNumberOfDashes)
                    StartCoroutine(Cooldown(cooldownTime));
                else
                {
                    dashState = DashState.Ready;
                    OnDashStateChanged?.Invoke(this, new OnDashStateChangedEventArgs { state = dashState });
                }
            }
        }
        /// <remarks>
        /// Dash is implemented as a coroutine because of the need to save coordinates
        /// We cannot save target location before moving to that location as we need to check if there are any colliders on the way
        /// </remarks>
        #endregion
    }
}
