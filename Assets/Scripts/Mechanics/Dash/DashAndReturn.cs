// <remarks>
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
    public class DashAndReturn : Dash, IStateMechanics<DashEventArgs>
    /*DEBUG statements for this document 
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
        public enum ReturnState
        {
            Active,
            InActive
        }



        public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
        public event EventHandler<OnReturnStateChangedEventArgs> OnReturnStateChanged;
        public class OnReturnStateChangedEventArgs : EventArgs
        {
            public MechanicsState state;
        }



        #region Serialized Fields
        [SerializeField] [Range(1, 5)] [Tooltip("Max number of dashed available at once")] private int maxNumberOfDashes = 5;
        [SerializeField] [Range(1, 5)] [Tooltip("Time dash remains inactive after being complete")] private float dashInterval = .3f;
        [Space]
        [Header("Cooldown settings")]
        [SerializeField] [Range(1f, 10f)] [Tooltip("Dash cooldown time in seconds")] private float cooldownTime = 3f;
        [SerializeField] [Range(1f, 10f)] [Tooltip("Dash cooldown time after every return in seconds")] private float returnCooldownTime = 2f;
        [SerializeField] [Range(0f, 10f)] [Tooltip("Dash cooldown time after last return in seconds")] private float lastReturnCooldownTime = .5f;
        [SerializeField] [Range(.1f, 5f)] [Tooltip("Time  after one dash untill coldown is triggered")] private float cooldownTriggeringTime = .8f;
        #endregion

        private float lastDashTime;

        private MechanicsState dashState;
        private MechanicsState returnState;

        private List<Vector3> savedCoordinates;

        public MechanicsState State { get { return dashState; } }

        private bool showDebug = true;
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>



        #region MonoBehaviour Callbacks
        protected override void Awake()
        {
            base.Awake();

            dashState = MechanicsState.Ready;
            returnState = MechanicsState.Ready;

            savedCoordinates = new List<Vector3>();
        }

        private void Update()
        {
            CheckCooldownTriggeringTime();
        }
        #endregion

        // Check if too much time passed since last dash
        private void CheckCooldownTriggeringTime()
        {
            if (dashState == MechanicsState.Ready && savedCoordinates.Count != 0)
            {
                if (lastDashTime - Time.deltaTime < 0)
                {
                    lastDashTime = cooldownTriggeringTime;
                    StartCoroutine(Cooldown(cooldownTime));
                }
                else
                    lastDashTime -= Time.deltaTime;
            }
            else
                lastDashTime = cooldownTriggeringTime;
        }

        #region Dash Overrides
        public override void Trigger(Vector3 direction)
        {
            if (dashState != MechanicsState.Ready)
                return;

            base.Trigger(direction);
        }

        protected override void Handle()
        {
            StartCoroutine(DashCoroutine(direction));
        }
        #endregion

        #region Return Mechanics
        public void TriggerReturn()
        {
            if (returnState == MechanicsState.Ready)
                HandleReturn();
        }

        private void HandleReturn()
        {
            gameObject.transform.position = savedCoordinates[savedCoordinates.Count - 1];
            savedCoordinates.RemoveAt(savedCoordinates.Count - 1);

            float cooldownTime;
            if (savedCoordinates.Count == 0)
            {
                returnState = MechanicsState.OnCooldown;
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
        #endregion



        #region Coroutines
        IEnumerator DashCoroutine(Vector3 direction)
        {
            dashState = MechanicsState.Active;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = dashState });
            base.Handle();
            yield return new WaitForFixedUpdate();
            savedCoordinates.Add(transform.position);

            // DEBUG
            if (showDebug)
                UtilsClass.DrawCross(transform.position, Color.yellow, 2f);

            returnState = MechanicsState.Ready;
            OnReturnStateChanged?.Invoke(this, new OnReturnStateChangedEventArgs { state = returnState });

            yield return new WaitForSeconds(dashInterval);
            /// <remarks>
            /// Shoud be set to animation time
            /// </remarks>

            if (savedCoordinates.Count == maxNumberOfDashes)
                StartCoroutine(Cooldown(cooldownTime));
            else
            {
                dashState = MechanicsState.Ready;
                OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = dashState });
            }
        }
        /// <remarks>
        /// Dash is implemented as a coroutine because of the need to save coordinates
        /// We cannot save target location before moving to that location as we need to check if there are any colliders on the way
        /// </remarks>

        IEnumerator Cooldown(float time)
        {
            dashState = MechanicsState.OnCooldown;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = dashState });

            yield return new WaitForSeconds(time); // wait for required amount of seconds

            savedCoordinates = new List<Vector3>();
            returnState = MechanicsState.OnCooldown;
            dashState = MechanicsState.Ready;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = dashState });
            OnReturnStateChanged?.Invoke(this, new OnReturnStateChangedEventArgs { state = returnState });
        }
        #endregion

    }
}