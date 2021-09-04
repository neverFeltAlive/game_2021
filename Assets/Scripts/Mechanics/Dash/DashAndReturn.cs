using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Custom.Utils;

namespace Custom.Mechanics
{
    /// <summary>
    /// 
    /// DashAndReturn is used for extending dash mechanics
    /// It adds these features to standart dash mechanics:
    /// a cooldown mechanics, 
    /// a posibolity to return to the destinations of dash
    /// a posibility to perform power dash
    /// :NeverFeltAlive
    /// 
    /// </summary>
    [RequireComponent(typeof(IDisablableMovement))]
    public class DashAndReturn : Dash, IOverLoadable
    {
        public event EventHandler<OnStateChangedEventArgs> OnDashStateChanged;
        public event EventHandler<OnStateChangedEventArgs> OnReturnStateChanged;



        #region Fields
        [SerializeField] private DashAndReturnStats stats;
        
        private bool _isOverload;

        private float lastDashTime;

        private State dashState;
        private State returnState;

        private List<Vector3> savedCoordinates;

        public bool IsOverload { set { _isOverload = value; } }
        #endregion 

        #region DEBUG
        /// <remarks>
        /// Set to true to show debug vectors
        /// </remarks>
        private bool showDebug = true;
        #endregion



        #region MonoBehaviour Callbacks
        protected override void Awake()
        {
            base.Awake();

            dashState = State.Ready;
            returnState = State.Ready;

            savedCoordinates = new List<Vector3>();

            _isOverload = false;
        }

        private void Update() =>
            CheckCooldownTriggeringTime();
        #endregion

        // Check if too much time passed since last dash
        private void CheckCooldownTriggeringTime()
        {
            if (dashState == State.Ready && savedCoordinates.Count != 0)
            {
                if (lastDashTime - Time.deltaTime < 0)
                {
                    lastDashTime = stats.cooldownTriggeringTime;
                    StartCoroutine(Cooldown(stats.cooldownTime));
                }
                else
                    lastDashTime -= Time.deltaTime;
            }
            else
                lastDashTime = stats.cooldownTriggeringTime;
        }

        public void TriggerDash(Vector3 direction, bool isPower = false)
        {
            if (dashState == State.Ready)
            {
                if (direction != Vector3.zero)
                    StartCoroutine(DashCoroutine(direction, isPower));
            }
        }

        public void Return()
        {
            if (returnState == State.Ready)
            {
                gameObject.transform.position = savedCoordinates[savedCoordinates.Count - 1];
                savedCoordinates.RemoveAt(savedCoordinates.Count - 1);

                // Check if no coordinates are left to return
                float cooldownTime;
                if (savedCoordinates.Count == 0)
                {
                    returnState = State.OnCooldown;
                    OnReturnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = returnState });
                    cooldownTime = stats.lastReturnCooldownTime;
                }
                else
                    cooldownTime = stats.returnCooldownTime;

                StopAllCoroutines();
                StartCoroutine(Cooldown(cooldownTime));
                /// We need to stop all coroutines to make last return completely restart cooldown with new timer
            }
        }



        #region Coroutines
        /// <summary>
        /// Performs dash and then waits for the next frame to save coordinates
        /// </summary>
        /// <param name="isPower">True if dash force needs to be multiplied</param>
        IEnumerator DashCoroutine(Vector3 direction, bool isPower)
        {
            float interval = .15f * Time.timeScale;
            float powerMultiplier;
            if (isPower || _isOverload)
                powerMultiplier = 3f;
            else
                powerMultiplier = 1f;
            IDisablableMovement movement = GetComponent<IDisablableMovement>();

            dashState = State.Active;
            OnDashStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = dashState });

            base.PerformDash(direction, FORCE * powerMultiplier);

            yield return new WaitForFixedUpdate();
            savedCoordinates.Add(transform.position);

            #region DEBUG   
            if (showDebug)
                UtilsClass.DrawCross(transform.position, Color.yellow, 2f);
            #endregion

            movement.DisableMovement();
            yield return new WaitForSeconds(interval);
            movement.EnableMovement();

            returnState = State.Ready;
            OnReturnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = returnState });

            // Check if no more dashes left
            if (savedCoordinates.Count == stats.maxNumberOfDashes)
                StartCoroutine(Cooldown(stats.cooldownTime));
            else
            {
                dashState = State.Ready;
                OnDashStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = dashState });
            }
        }

        IEnumerator Cooldown(float time)
        {
            dashState = State.OnCooldown;
            OnDashStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = dashState });

            yield return new WaitForSeconds(time); // wait for required amount of seconds

            savedCoordinates = new List<Vector3>();
            returnState = State.OnCooldown;
            dashState = State.Ready;
            OnDashStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = dashState });
            OnReturnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = returnState });
        }
        #endregion

    }
}